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
        GetEntityInit(scene.Entities, sb, usings, null);

        var code =
$$"""
{{usingBlock}}

namespace DualBlade.Scenes;

internal partial class {{name}} : YamlGameScene
{
    public {{name}}(IGameContext context) : base(context) {}

    {{probsSb}}

    protected override IEnumerable<EntityBuilder> SetupEntities()
    {
        {{sb}}
    }
}
""";

        return code;
    }

    private void GetEntityProperties(IEnumerable<YamlEntity> entities, StringBuilder sb)
    {
        foreach (var e in entities.Where(x => x.Name is not null))
        {
            GetEntityProperties(e.Children, sb);
            sb.AppendLine($"public {e.Type} {e.Name} {{ get; private set; }}");
        }
    }

    private void GetEntityInit(IEnumerable<YamlEntity> entities, StringBuilder sb, HashSet<string> usings, string parentBuilder)
    {
        foreach (var e in entities)
        {
            var name = e.Name ?? e.Type + Guid.NewGuid().ToString().Replace("-", "");
            var namePrefix = e.Name is null ? "var " : "this.";

            var ctorParametersArray = e.Ctor?.Select(x => x.ToString()).ToArray() ?? [];
            var ctorParameters = string.Join(",", ctorParametersArray);

            sb.Append(
            $$"""
            {{namePrefix}}{{e.Name}} = new {{e.Type}}({{ctorParameters}});
            {{string.Join("\n", e.Properties.Select(p => $"{e.Name}.{p.Key} = {p.Value};"))}}
            """);

            if (parentBuilder is not null)
            {
                sb.AppendLine($"var {e.Name}Builder = {parentBuilder}.AddChild();");
            }
            else
            {
                sb.AppendLine($"var {e.Name}Builder = CreateEntity({e.Name});");
            }

            foreach (var c in e.Components)
            {
                AddComponent(name, c, sb);
            }

            foreach (var c in e.Children)
            {
                sb.AppendLine($"{e.Name}.AddChild({c.Name});");
            }

            GetEntityInit(e.Children, sb, usings, $"{e.Name}Builder");

            sb.AppendLine($"yield return {e.Name}Builder;");
        }
    }

    private void AddComponent(string entityName, YamlComponent component, StringBuilder sb)
    {
        sb.AppendLine($"// {component.Type}");
        sb.AppendLine("{");
        sb.AppendLine($"var component = new {component.Type}();");
        foreach (var p in component.Properties)
        {
            sb.AppendLine($"component.{p.Key} = {p.Value};");
        }
        sb.AppendLine($"{entityName}.AddComponent(component);");
        sb.AppendLine("}");
    }
}
