using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoGamesEngine.Analyzer;

[GenerateAutomaticInterface]
public class SceneCodeGenerator : ISceneCodeGenerator
{
    public string GenerateCode(IEnumerable<IEntity> entities, string name)
    {
        var probsSb = new StringBuilder();
        GetEntityProperties(entities, probsSb);
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

    {{probsSb}}

    protected override IEnumerable<IEntity> SetupEntities()
    {
        {{sb}}
    }
}
""";

        return code;
    }

    private void GetEntityProperties(IEnumerable<IEntity> entities, StringBuilder sb)
    {
        foreach (var e in entities.Where(x => x.Name is not null))
        {
            GetEntityProperties(e.Children, sb);
            sb.AppendLine($"public {e.Type} {e.Name} {{ get; private set; }}");
        }
    }

    private void GetEntityInit(IEnumerable<IEntity> entities, StringBuilder sb)
    {
        foreach (var e in entities)
        {
            GetEntityInit(e.Children, sb);
            var name = e.Name ?? e.Type + Guid.NewGuid().ToString().Replace("-", "");
            var namePrefix = e.Name is null ? "var " : "this.";

            sb.Append(
                $$"""
                    {{namePrefix}}{{e.Name}} = new {{e.Type}}()
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
