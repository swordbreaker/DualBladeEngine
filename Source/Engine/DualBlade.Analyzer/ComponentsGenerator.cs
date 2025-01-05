using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using static DualBlade.Analyzer.GeneratorUtils;

namespace DualBlade.Analyzer;

[Generator]
public class ComponentsGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var componentProvider = context.SyntaxProvider.CreateSyntaxProvider(IsComponent, GetECInfo);

        context.RegisterSourceOutput(componentProvider, (spc, info) =>
        {
            var code = GenerateComponentCode(info);
            code = FormatSource(code);
            spc.AddSource($"{info.StructName}.generated.cs", code);
        });
    }

    private string GenerateComponentCode(ComponentInfo info)
    {
        var (structName, ns, usings, hasDefaultCtor, fields, methods) = info;

        var ctor = !hasDefaultCtor ? $"public {structName}() {{ }}" : "";
        var usingBlock = string.Join("\n", usings);
        var allMethods = methods.Select(m => m.Identifier.ToString()).ToImmutableHashSet();

        var fieldSetters = fields.Select(f =>
        {
            var fieldName = f.Declaration.Variables.First().Identifier.Text;

            if (allMethods.Contains($"Set{fieldName}"))
            {
                return string.Empty;
            }

            var type = f.Declaration.Type.ToString();
            return $$"""
                public {{structName}} Set{{fieldName}}({{type}} newValue)
                {
                    this.{{fieldName}} = newValue;
                    return this;
                }
            """;
        });

        var fieldBlock = string.Join("\n", fieldSetters);

        return $$"""
            using DualBlade.Core.Components;
            using System.Runtime.InteropServices;
            {{usingBlock}}

            namespace {{ns}};

            [StructLayout(LayoutKind.Auto)]
            public partial struct {{structName}}
            {
                {{ctor}}

                public int Id { get; set; }
                public int EntityId { get; set; }

                {{fieldBlock}}
            }
            """;
    }

    private bool IsComponent(SyntaxNode node, CancellationToken token)
    {
        if (node is not StructDeclarationSyntax s) return false;

        var inheritsIComponent = s.BaseList?.Types.Any(x => x.Type.ToString() == "IComponent") ?? false;
        var isPartial = s.Modifiers.Any(x => x.ToString() == "partial");
        return inheritsIComponent && isPartial;
    }
}
