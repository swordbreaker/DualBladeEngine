using MonoGameEngine.Engine.Worlds;
using System;
using System.Collections.Generic;
using IComponent = MonoGameEngine.Engine.Components.IComponent;

namespace MonoGameEngine.Engine.Entities;

public interface IEntity
{
    IWorld World { init; get; }
    IEnumerable<IComponent> Components { get; }
    TComponent AddComponent<TComponent>() where TComponent : IComponent, new();
    void AddComponent<TComponent>(TComponent component) where TComponent : IComponent;
    void RemoveComponent<TComponent>() where TComponent : IComponent;
    void RemoveComponent(Type type);
}
