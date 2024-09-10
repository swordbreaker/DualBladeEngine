using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoGamesEngine.Analyzer;

[GenerateAutomaticInterface]
public class SceneCodeGenerator : ISceneCodeGenerator
{
    public string GenerateCode(IEnumerable<IEntity> entities, string name)
    {
        var sb = new StringBuilder();
        GetEntityInit(entities, sb);

        var code =
$$"""
using MonoGameEngine.Engine.Entities;
using MonoGameEngine.Engine.Worlds;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using MonoGameEngine.Engine.EntityHierarchy;
using MonoGameEngine.Engine.Extensions;

namespace MonoGameEngine.Scenes;

internal class {{name}} : YamlGameScene
{
    public {{name}}(IWorld world) : base(world) {}


    protected override IEnumerable<IEntity> SetupEntities()
    {
        {{sb}}
    }
}
""";

        return code;
    }

    private void GetEntityInit(IEnumerable<IEntity> entities, StringBuilder sb)
    {
        foreach (var e in entities)
        {
            GetEntityInit(e.Children, sb);

            sb.Append(
                $$"""
                    var {{e.Name}} = new {{e.Type}}()
                    {
                        World = World,
                        {{string.Join(",\n", e.Properties.Select(p => $"{p.Key} = {p.Value}"))}}
                    };

                    {{e.Name}}.Transform.Position = new Vector2({{e.Position[0]}}, {{e.Position[1]}});
                    {{e.Name}}.Transform.Scale = new Vector2({{e.Scale[0]}}, {{e.Scale[1]}});
                    {{e.Name}}.Transform.Rotation = {{e.Rotation}};

                    """);

            foreach (var c in e.Children)
            {
                sb.AppendLine($"{e.Name}.AddChild({c.Name});");
            }

            sb.AppendLine($"yield return {e.Name};");
        }
    }
}
