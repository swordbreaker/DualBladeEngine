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

    public static ComponentInfo GetECInfo(GeneratorSyntaxContext context, CancellationToken _)
    {
        var structDeclaration = (StructDeclarationSyntax)context.Node;
        var symbol = context.SemanticModel.GetDeclaredSymbol(structDeclaration);
        var ns = symbol.ContainingNamespace.ToString();
        var usings = context.Node.SyntaxTree.GetRoot().DescendantNodes().OfType<UsingDirectiveSyntax>().Select(x => x.ToString());
        var hasDefaultCtor = structDeclaration.Members.OfType<ConstructorDeclarationSyntax>().Any(x => x.ParameterList.Parameters.Count == 0);
        var fields = structDeclaration.Members.OfType<FieldDeclarationSyntax>();
        var methods = structDeclaration.Members.OfType<MethodDeclarationSyntax>();
        return new(structDeclaration.Identifier.Text, ns, usings, hasDefaultCtor, fields, methods);
    }
}
