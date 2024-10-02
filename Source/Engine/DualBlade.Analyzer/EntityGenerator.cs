using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System.Text;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

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
            var componentsToAdd = structDeclaration.AttributeLists.SelectMany(x => x.Attributes)
                .Select(x => context.SemanticModel.GetTypeInfo(x).Type)
                .Where(x => x.Name == "AddComponentAttribute")
                .Select(x => ((INamedTypeSymbol)x).TypeArguments[0])
                // get full name
                .Select(x => x.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));

            return (structName: structDeclaration.Identifier.Text, ns, hasDefaultCtor, componentsToAdd);
        });

        context.RegisterSourceOutput(componentProvider, (spc, tuple) =>
        {
            var (structName, ns, hasDefaultCtor, componentsToAdd) = tuple;
            var code = GenerateEntityCode(structName, ns, hasDefaultCtor, componentsToAdd);
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

    private string GenerateEntityCode(string structName, string ns, bool hasDefaultCtor, IEnumerable<string> componentsToAdd)
    {
        var ctor = !hasDefaultCtor ? $"public {structName}() {{ }}" : "";

        // public readonly ComponentProxy<TransformComponent> Transform => this.Component<TransformComponent>();
        var componentsProperties = string.Join("\n", componentsToAdd.Select(x => $"public readonly ComponentProxy<{x}> {x.Split('.').Last()} => this.Component<{x}>();"));
        var componentsAddStatement = string.Join("\n", componentsToAdd.Select(x => $"AddComponent(new {x}());"));

        return $$"""
            using System;
            using DualBlade.Core.Collections;
            using DualBlade.Core.Components;
            using DualBlade.Core.Worlds;
            using DualBlade.Core.Entities;
            using System.Collections.Generic;
            using System.Linq;
            using DualBlade.Core.Utils;
            using static DualBlade.Core.Entities.IEntity;
            using DualBlade.Core.Extensions;

            namespace {{ns}};

            public partial struct {{structName}} : IEntity
            {
                {{ctor}}

                {{componentsProperties}}

                /// <inheritdoc />
                public int Id { get; private set; } = -1;

                /// <inheritdoc />
                public Memory<Type> ComponentTypes { get; private set; }

                /// <inheritdoc />
                public GrowableMemory<IComponent> InternalComponents { get; } = new(5);

                /// <inheritdoc />
                public readonly IEnumerable<IComponent> Components => InternalComponents.ToSpan().ToArray();

                /// <inheritdoc />
                public int Parent { get; set; } = -1;

                /// <inheritdoc />
                public GrowableMemory<int> Children { get; set; } = new(1);

                /// <inheritdoc />
                public void Init(int id)
                {
                    this.Id = id;
                    {{componentsAddStatement}}
                }

                /// <inheritdoc />
                public readonly ComponentProxy<TComponent> Component<TComponent>() where TComponent : IComponent
                {
                    if (InternalComponents.TryFind(x => x is TComponent, out var component))
                    {
                        return new ComponentProxy<TComponent>(UpdateComponent, (TComponent)component);
                    }

                    throw new InvalidOperationException($"Component {typeof(TComponent).Name} not found on entity {Id}");
                }

                /// <inheritdoc />
                public readonly bool HasComponent<TComponent>() where TComponent : IComponent =>
                    InternalComponents.TryFind(x => x is TComponent, out _);

                /// <inheritdoc />
                public readonly bool TryGetComponent<TComponent>(out ComponentProxy<TComponent> componentProxy) where TComponent : IComponent
                {
                    if (InternalComponents.TryFind(x => x is TComponent, out var comp))
                    {
                        componentProxy = new ComponentProxy<TComponent>(UpdateComponent, (TComponent)comp);
                        return true;
                    }
                    componentProxy = default;
                    return false;
                }

                /// <inheritdoc />
                public readonly void UpdateComponent<TComponent>(TComponent component) where TComponent : IComponent
                {
                    InternalComponents[component.Id] = component;
                }

                /// <inheritdoc />
                public ComponentProxy<TComponent> AddComponent<TComponent>(TComponent component) where TComponent : IComponent
                {
                    if (ComponentTypes.Span.Contains(typeof(TComponent)))
                    {
                        throw new InvalidOperationException($"Component {typeof(TComponent).Name} already exists on entity {Id}");
                    }

                    var comps = this.Components.Append(component).OrderBy(x => x.GetType(), new SimpleTypeComparer()).ToArray();
                    var types = comps.Select(x => x.GetType()).ToArray();
                    this.InternalComponents.Clear();

                    int compId = -1;
                    for (int i = 0; i < comps.Count(); i++)
                    {
                        comps[i].Id = i;
                        this.InternalComponents.Add(comps[i]);
                        if (this.InternalComponents[i].GetType() == component.GetType())
                        {
                            compId = i;
                        }
                    }

                    this.ComponentTypes = new Memory<Type>(types);
                    return new ComponentProxy<TComponent>(UpdateComponent, (TComponent)this.InternalComponents[compId]);
                }

                /// <inheritdoc />
                public void RemoveComponent<TComponent>() where TComponent : IComponent
                {
                    var comps = this.Components.Where(x => x.GetType() != typeof(TComponent)).ToArray();
                    var types = comps.Select(x => x.GetType()).ToArray();
                    this.InternalComponents.Clear();

                    for (int i = 0; i < comps.Count(); i++)
                    {
                        comps[i].Id = i;
                        this.InternalComponents.Add(comps[i]);
                    }

                    this.ComponentTypes = new Memory<Type>(types);
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
