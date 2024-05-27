using Microsoft.CodeAnalysis;
using System.IO;
using System.Linq;

namespace MonoGamesEngine.Analyzer
{
    [Generator]
    public class YamlSceneGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var parser = new SceneParser();

            var textFiles = context.AdditionalTextsProvider.Where(static file => file.Path.EndsWith(".scene.yaml"));
            
            var namesAndContents = textFiles.Select((text, cancellationToken) =>
                (name: Path.GetFileNameWithoutExtension(text.Path),
                entity: parser.ParseScene(text.GetText(cancellationToken)!.ToString())));

            context.RegisterSourceOutput(namesAndContents, (spc, nameAndContent) =>
            {
                var (name, entity) = nameAndContent;

                spc.AddSource($"{name}.cs", 
$$"""
using MonoGameEngine.Engine.Entities;
using MonoGameEngine.Engine.Worlds;
using System.Collections.Generic;

namespace MonoGameEngine.Engine.EntityHierarchy;

internal class {{name}} : YamlGameScene
{
    public Test(IWorld world) : base(world) {}


    protected override IEnumerable<IEntity> SetupEntities()
    {
        yield return new {{entity.Type}}()
        {
            World = World,
            Position = new Vector2({{entity.Position[0]}}, {{entity.Position[1]}}),
            Scale = new Vector2({{entity.Scale[0]}}, {{entity.Scale[1]}}),
            Rotation = {{entity.Rotation}},
            {{entity.Properties.Select(p => $"{p.Key} = {p.Value},")}}
        };
    }
}
""");
            });
        }
    }
}
