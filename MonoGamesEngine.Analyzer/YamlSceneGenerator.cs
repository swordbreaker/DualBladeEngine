using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace MonoGamesEngine.Analyzer
{
    [Generator]
    public class YamlSceneGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            try
            {
                var parser = new SceneParser();
                var codeGenerator = new SceneCodeGenerator();

                var textFiles = context.AdditionalTextsProvider.Where(static file => file.Path.EndsWith(".scene.yaml"));

                var namesAndContents = textFiles.Select((text, cancellationToken) =>
                    (name: Path.GetFileNameWithoutExtension(text.Path),
                    entity: parser.ParseScene(text.GetText(cancellationToken)!.ToString())));

                context.RegisterSourceOutput(namesAndContents, (spc, nameAndContent) =>
                {
                    var (name, entities) = nameAndContent;
                    name = name.Replace(".scene", "");

                    var code = codeGenerator.GenerateCode(entities, name);

                    code = FormatSource(code);
                    spc.AddSource($"{name}.generated.cs", code);
                });
            }
            catch (Exception e)
            {
                context.RegisterSourceOutput(context.AdditionalTextsProvider, (spc, text) =>
                {
                    var descriptor = new DiagnosticDescriptor("YAMLGEN001", "YAMLGEN", $"Error generating scene {e.Message} {e.StackTrace}", "YAMLGEN", DiagnosticSeverity.Error, true);
                    spc.ReportDiagnostic(Diagnostic.Create(descriptor, null, "Error generating scene", DiagnosticSeverity.Error));
                });
            }
        }

        private string FormatSource(string code)
        {
            var sourceText = SourceText.From(code, Encoding.UTF8);
            var tree = CSharpSyntaxTree.ParseText(sourceText);
            var node = tree.GetRoot().NormalizeWhitespace();
            return node.GetText(Encoding.UTF8).ToString();
        }
    }
}
