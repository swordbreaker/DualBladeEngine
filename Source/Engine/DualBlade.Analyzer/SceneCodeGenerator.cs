using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DualBlade.Analyzer;

[GenerateAutomaticInterface]
public class SceneCodeGenerator : ISceneCodeGenerator
{
    public string GenerateCode(SceneRoot scene, string name)
    {
        var usings = new HashSet<string>(
            [
            "DualBlade.Core.Entities",
            "DualBlade.Core.Services",
            "DualBlade.Core.Worlds",
            "Microsoft.Xna.Framework",
            "System.Collections.Generic",
            "DualBlade.Core.Extensions",
            "DualBlade.Core.Scenes"
            ]);

        foreach (var item in scene.AdditionalUsings)
        {
            usings.Add(item);
        }

        var usingBlock = string.Join("\n", usings.Select(x => $"using {x};"));

        var probsSb = new StringBuilder();
        GetEntityProperties(scene.Entities, probsSb);
        var sb = new StringBuilder();
        GetEntityInit(scene.Entities, sb, usings);

        var code =
$$"""
{{usingBlock}}

namespace DualBlade.Scenes;

internal partial class {{name}} : YamlGameScene
{
    public {{name}}(IGameContext context) : base(context) {}

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

    private void GetEntityInit(IEnumerable<IEntity> entities, StringBuilder sb, HashSet<string> usings)
    {
        foreach (var e in entities)
        {
            GetEntityInit(e.Children, sb, usings);
            var name = e.Name ?? e.Type + Guid.NewGuid().ToString().Replace("-", "");
            var namePrefix = e.Name is null ? "var " : "this.";

            sb.Append(
                $$"""
                    {{namePrefix}}{{e.Name}} = new {{e.Type}}();
                    {{string.Join("\n", e.Properties.Select(p => $"{e.Name}.{p.Key} = {p.Value};"))}}

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
