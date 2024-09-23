﻿using DualBlade.Core.Components;

namespace DualBlade.Core.Entities;

public class Entity : IEntity
{
    private readonly Dictionary<Type, IComponent> _components = [];

    public IEnumerable<IComponent> Components => _components.Values;

    public void AddComponent<TComponent>(TComponent component) where TComponent : IComponent =>
        _components.Add(typeof(TComponent), component);

    public void RemoveComponent<TComponent>() where TComponent : IComponent =>
        RemoveComponent(typeof(TComponent));

    public void RemoveComponent(Type type) =>
        _components.Remove(type);

    public TComponent AddComponent<TComponent>() where TComponent : IComponent, new()
    {
        var component = new TComponent() { Entity = this };
        _components.Add(typeof(TComponent), component);
        return component;
    }

    protected TComponent AddComponent<TComponent>(Func<IEntity, TComponent> factory) where TComponent : IComponent
    {
        var component = factory(this);
        _components.Add(typeof(TComponent), component);
        return component;
    }
}