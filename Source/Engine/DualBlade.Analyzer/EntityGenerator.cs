using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System.Text;
using System.Threading;
using System.Linq;

namespace DualBlade.Analyzer;

[Generator]
public class EntityGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var componentProvider = context.SyntaxProvider.CreateSyntaxProvider(IsEntity, (context, token) =>
        {
            var structDeclaration = (StructDeclarationSyntax)context.Node;
            var symbol = context.SemanticModel.GetDeclaredSymbol(structDeclaration);
            var ns = symbol.ContainingNamespace.ToString();
            var hasDefaultCtor = structDeclaration.Members.OfType<ConstructorDeclarationSyntax>().Any(x => x.ParameterList.Parameters.Count == 0);
            return (structName: structDeclaration.Identifier.Text, ns, hasDefaultCtor);
        });

        context.RegisterSourceOutput(componentProvider, (spc, tuple) =>
        {
            var (structName, ns, hasDefaultCtor) = tuple;
            var code = GenerateEntityCode(structName, ns, hasDefaultCtor);
            code = FormatSource(code);
            spc.AddSource($"{structName}.generated.cs", code);
        });
    }

    private string FormatSource(string code) =>
        CSharpSyntaxTree.ParseText(SourceText.From(code, Encoding.UTF8))
            .GetRoot()
            .NormalizeWhitespace()
            .GetText(Encoding.UTF8)
            .ToString();

    private string GenerateEntityCode(string structName, string ns, bool hasDefaultCtor)
    {
        var ctor = !hasDefaultCtor ? $"public {structName}() {{ }}" : "";

        return $$"""
            using System;
            using DualBlade.Core.Collections;
            using DualBlade.Core.Components;
            using DualBlade.Core.Entities;
            using DualBlade.Core.Worlds;

            namespace {{ns}};

            public partial struct {{structName}} : IEntity
            {
                private GrowableMemory<(Type, int)> componentsIds = new(100);
                private World.AddComponentDelegate? addComponent;

                {{ctor}}

                public int Id { get; private set; }
                public GrowableMemory<ComponentRef<IComponent>> Components { get; }
                public GrowableMemory<IComponent> InitialComponents { get; }

                public readonly void AddComponent(IComponent component)
                {
                    if (componentsIds.Contains(x => x.Item1 == component.GetType()))
                    {
                        throw new InvalidOperationException("Component already added");
                    }

                    if (this.addComponent is not null)
                    {
                        this.Components.Add(this.addComponent(component));
                    }
                    else
                    {
                        this.InitialComponents.Add(component);
                    }
                }

                public readonly ComponentRef<TComponent>? Component<TComponent>() where TComponent : IComponent
                {
                    if(this.Components.TryFind(x => x.GetCopy().GetType() == typeof(TComponent), out var cRef))
                    {
                        return cRef.As<TComponent>();
                    }
                    
                    return default;
                }

                public void Init(World.AddComponentDelegate addComponent, int id)
                {
                    this.Id = id;
                    this.addComponent = addComponent;
                }
            }
            """;
    }

    private bool IsEntity(SyntaxNode node, CancellationToken token)
    {
        if (node is not StructDeclarationSyntax s) return false;

        var inheritsIComponent = s.BaseList?.Types.Any(x => x.Type.ToString() == "IEntity") ?? false;
        var isPartial = s.Modifiers.Any(x => x.ToString() == "partial");
        return inheritsIComponent && isPartial;
    }
}
