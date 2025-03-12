using DualBlade.Analyzer;
using DualBlade.Core.Entities;
using DualBlade.Core.Scenes;
using DualBlade.Core.Services;
using System;
using System.Collections.Generic;
using DualBlade.Core.Components;
using DualBlade._2D.Rendering.Components;
using System.Text.RegularExpressions;
using System.Linq;
using Microsoft.Xna.Framework;

namespace DualBlade.Editor.Player.Yaml;
public partial class SceneGenerator(IGameContext gameContext)
{
    private IEcsManager Ecs = gameContext.EcsManager;

    public IGameScene Create(SceneRoot sceneRoot)
    {
        var usings = new HashSet<string>(
        [
            "DualBlade.Core.Entities",
            "DualBlade.Core.Services",
            "DualBlade._2D.Rendering.Components",
            "DualBlade.Core.Worlds",
            "Microsoft.Xna.Framework",
            "System.Collections.Generic",
            "DualBlade.Core.Extensions",
            "DualBlade.Core.Scenes"
        ]);

        sceneRoot.AdditionalUsings.ForEach(x => usings.Add(x));

        var root = new EntityBuilder(new RootEntity());
        foreach (var entityDto in sceneRoot.Entities)
        {
            CreateEntity(entityDto, usings, root);
        }

        var scene = new EmptyScene(gameContext, root);

        return scene;
    }

    private void CreateEntity(EntityDto entityDto, HashSet<string> usings, EntityBuilder parent)
    {
        var entityType = FindType(usings, entityDto.Type) ?? throw new Exception($"Type {entityDto.Type} not found");

        var ctorParameters = CreateCtorParameters(entityDto.Ctor).ToArray();
        var entity = Activator.CreateInstance(entityType, ctorParameters) as IEntity;

        foreach (var (key, value) in entityDto.Properties)
        {
            var prop = entityType.GetProperty(key);
            prop?.SetValue(entity, value);
        }

        foreach (var componentDto in entityDto.Components)
        {
            var component = CreateComponent(componentDto, usings, entity);
            entity.AddComponent(component);
        }

        var currentBuilder = parent.AddChild(entity);

        foreach (var childDto in entityDto.Children)
        {
            CreateEntity(childDto, usings, currentBuilder);
        }
    }

    private IEnumerable<object> CreateCtorParameters(IEnumerable<object> ctorParameters)
    {
        foreach (var param in ctorParameters)
        {
            if (param is string s)
            {
                s = s.Trim();
                if (s.StartsWith("new Vector2"))
                {
                    var matches = Vector2CoordinateRegex().Match(s);
                    var coordinates = matches.Groups[1].Value.Split(',').Select(x => float.Parse(x)).ToArray();
                    yield return new Vector2(coordinates[0], coordinates[1]);
                }
                else if (s.Contains("GameContext"))
                {
                    yield return gameContext;
                }
                else
                {
                    yield return s;
                }
            }
            else
            {
                yield return param;
            }
        }
    }

    private IComponent CreateComponent(ComponentDto componentDto, HashSet<string> usings, IEntity entity)
    {
        var componentType = FindType(usings, componentDto.Type) ?? throw new Exception($"Type {componentDto.Type} not found");
        var comp = Activator.CreateInstance(componentType) as IComponent;

        componentType.GetProperty("Entity")?.SetValue(comp, entity);

        foreach (var (key, value) in componentDto.Properties)
        {
            if (comp is RenderComponent renderComponent && key == "Texture")
            {
                renderComponent.SetSprite(gameContext.GameEngine.CreateSprite(value as string));
            }

            var prop = componentType.GetProperty(key);
            prop?.SetValue(comp, value);
        }

        return comp;
    }

    private static Type? FindType(IEnumerable<string> namespaces, string type)
    {
        foreach (var ns in namespaces)
        {
            foreach (var assemblies in AppDomain.CurrentDomain.GetAssemblies())
            {
                var t = assemblies.GetType($"{ns}.{type}");
                if (t is not null)
                {
                    return t;
                }
            }
        }
        return null;
    }

    [GeneratedRegex(@"\(([^)]*)\)")]
    private static partial Regex Vector2CoordinateRegex();
}
