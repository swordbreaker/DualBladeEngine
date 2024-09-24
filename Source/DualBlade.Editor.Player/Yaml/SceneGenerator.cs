using DualBlade._2D.Rendering.Entities;
using DualBlade.Analyzer;
using DualBlade.Core.Entities;
using DualBlade.Core.Scenes;
using DualBlade.Core.Services;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using DualBlade.Core.Components;
using DualBlade._2D.Rendering.Components;

namespace DualBlade.Editor.Player.Yaml;
public class SceneGenerator(IGameContext gameContext)
{
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

        var root = new RootEntity();
        foreach (var entityDto in sceneRoot.Entities)
        {
            CreateEntity(entityDto, usings, root);
        }

        var scene = new EmptyScene(gameContext);
        scene.SetRoot(root);

        return scene;
    }

    private IEntity CreateEntity(EntityDto entityDto, HashSet<string> usings, IEntity? parent)
    {
        var entityType = FindType(usings, entityDto.Type) ?? throw new Exception($"Type {entityDto.Type} not found");
        var entity = Activator.CreateInstance(entityType) as IEntity;
        if (entity is TransformEntity transformEntity)
        {
            transformEntity.Transform.Position = new Vector2(entityDto.Position[0], entityDto.Position[1]);
            transformEntity.Transform.Scale = new Vector2(entityDto.Scale[0], entityDto.Scale[1]);
            transformEntity.Transform.Rotation = entityDto.Rotation;
        }

        foreach (var (key, value) in entityDto.Properties)
        {
            var prop = entityType.GetProperty(key);
            prop?.SetValue(entity, value);
        }

        if (parent is INodeEntity nodeEntity)
        {
            nodeEntity.AddChild(entity);
        }

        foreach (var componentDto in entityDto.Components)
        {
            var component = CreateComponent(componentDto, usings, entity);
            entity.AddComponent(component);
        }

        foreach (var childDto in entityDto.Children)
        {
            CreateEntity(childDto, usings, entity);
        }

        return entity;
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

    private Type? FindType(IEnumerable<string> namespaces, string type)
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
}
