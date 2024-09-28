using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Linq;
using System.Text;
using System.Threading;

namespace DualBlade.Analyzer;
public static class GeneratorUtils
{
    public static string FormatSource(string code) =>
        CSharpSyntaxTree.ParseText(SourceText.From(code, Encoding.UTF8))
            .GetRoot()
            .NormalizeWhitespace()
            .GetText(Encoding.UTF8)
            .ToString();

    public static (string structName, string ns, bool hasDefaultCtor) GetECInfo(GeneratorSyntaxContext context, CancellationToken token)
    {
        var structDeclaration = (StructDeclarationSyntax)context.Node;
        var symbol = context.SemanticModel.GetDeclaredSymbol(structDeclaration);
        var ns = symbol.ContainingNamespace.ToString();
        var hasDefaultCtor = structDeclaration.Members.OfType<ConstructorDeclarationSyntax>().Any(x => x.ParameterList.Parameters.Count == 0);
        return (structName: structDeclaration.Identifier.Text, ns, hasDefaultCtor);
    }
}
