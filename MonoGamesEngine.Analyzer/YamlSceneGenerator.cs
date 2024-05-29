using Microsoft.CodeAnalysis;
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

                var textFiles = context.AdditionalTextsProvider.Where(static file => file.Path.EndsWith(".scene.yaml"));

                var namesAndContents = textFiles.Select((text, cancellationToken) =>
                    (name: Path.GetFileNameWithoutExtension(text.Path),
                    entity: parser.ParseScene(text.GetText(cancellationToken)!.ToString())));

                context.RegisterSourceOutput(namesAndContents, (spc, nameAndContent) =>
                {
                    var (name, entities) = nameAndContent;
                    name = name.Replace(".scene", "");

                    var sb = new StringBuilder();
                    foreach(var e in entities)
                    {
                        sb.AppendLine($"yield return new {e.Type}()");
                        sb.AppendLine("{");
                        sb.AppendLine($"    World = World,");
                        sb.AppendLine($"    Position = new Vector2({e.Position[0]}, {e.Position[1]}),");
                        sb.AppendLine($"    Scale = new Vector2({e.Scale[0]}, {e.Scale[1]}),");
                        sb.AppendLine($"    Rotation = {e.Rotation},");
                        foreach(var p in e.Properties)
                        {
                            sb.AppendLine($"    {p.Key} = {p.Value},");
                        }
                        sb.AppendLine("};");
                    }

                    spc.AddSource($"{name}.generated.cs",
    $$"""
using MonoGameEngine.Engine.Entities;
using MonoGameEngine.Engine.Worlds;
using System.Collections.Generic;

namespace MonoGameEngine.Scenes;

internal class {{name}} : YamlGameScene
{
    public {{name}}(IWorld world) : base(world) {}


    protected override IEnumerable<IEntity> SetupEntities()
    {
        {{sb.ToString()}}
    }
}
""");
                });
            }
            catch(Exception e)
            {
                context.RegisterSourceOutput(context.AdditionalTextsProvider, (spc, text) =>
                {
                    var descriptor = new DiagnosticDescriptor("YAMLGEN001", "YAMLGEN", $"Error generating scene {e.Message}", "YAMLGEN", DiagnosticSeverity.Error, true);
                    spc.ReportDiagnostic(Diagnostic.Create(descriptor, null, "Error generating scene", DiagnosticSeverity.Error));
                });
            }
        }
    }
}
