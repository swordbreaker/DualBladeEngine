using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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

        context.RegisterSourceOutput(componentProvider, (spc, tuple) =>
        {
            var (structName, ns, hasDefaultCtor) = tuple;
            var code = GenerateComponentCode(structName, ns, hasDefaultCtor);
            code = FormatSource(code);
            spc.AddSource($"{structName}.generated.cs", code);
        });
    }

    private string GenerateComponentCode(string structName, string ns, bool hasDefaultCtor)
    {
        var ctor = !hasDefaultCtor ? $"public {structName}() {{ }}" : "";

        return $$"""
            using DualBlade.Core.Components;
            using System.Runtime.InteropServices;

            namespace {{ns}};

            [StructLayout(LayoutKind.Auto)]
            public partial struct {{structName}}
            {
                {{ctor}}

                public int Id { get; set; }
                public int EntityId { get; set; }
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
