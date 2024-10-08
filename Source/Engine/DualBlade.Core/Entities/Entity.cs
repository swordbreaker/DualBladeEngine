﻿using DualBlade.Core.Collections;
using DualBlade.Core.Components;
using DualBlade.Core.Utils;
using DualBlade.Core.Worlds;
using DualBlade.Core.Extensions;

namespace DualBlade.Core.Entities;

public struct Entity : IEntity
{
    /// <inheritdoc />
    public int Id { get; private set; } = -1;

    public Entity()
    {

    }

    /// <inheritdoc />
    public Memory<Type> ComponentTypes { get; private set; }

    /// <inheritdoc />
    public GrowableMemory<IComponent> InternalComponents { get; } = new(5);

    /// <inheritdoc />
    public readonly IEnumerable<IComponent> Components => InternalComponents.ToSpan().ToArray();

    /// <inheritdoc />
    public int Parent { get; set; } = -1;

    /// <inheritdoc />
    public GrowableMemory<int> Children { get; set; } = new(1);

    /// <inheritdoc />
    public void Init(int id)
    {
        this.Id = id;
        foreach (var component in this.InternalComponents.ToSpan())
        {
            component.EntityId = this.Id;
        }
    }

    /// <inheritdoc />
    public readonly TComponent Component<TComponent>() where TComponent : IComponent
    {
        if (InternalComponents.TryFind(x => x is TComponent, out var component))
        {
            return (TComponent)component;
        }

        throw new InvalidOperationException($"Component {typeof(TComponent).Name} not found on entity {Id}");
    }

    /// <inheritdoc />
    public readonly bool HasComponent<TComponent>() where TComponent : IComponent =>
        InternalComponents.TryFind(x => x is TComponent, out _);

    /// <inheritdoc />
    public readonly bool TryGetComponent<TComponent>(out TComponent componentProxy) where TComponent : IComponent
    {
        if (InternalComponents.TryFind(x => x is TComponent, out var comp))
        {
            componentProxy = (TComponent)comp;
            return true;
        }
        componentProxy = default;
        return false;
    }

    /// <inheritdoc />
    public readonly void UpdateComponent<TComponent>(TComponent component) where TComponent : IComponent
    {
        component.EntityId = this.Id;
        InternalComponents[component.Id] = component;
    }

    /// <inheritdoc />
    public ComponentProxy<TComponent> AddComponent<TComponent>(TComponent component) where TComponent : IComponent
    {
        if (ComponentTypes.Span.Contains(typeof(TComponent)))
        {
            throw new InvalidOperationException($"Component {typeof(TComponent).Name} already exists on entity {Id}");
        }

        component.EntityId = this.Id;
        var comps = this.Components.Append(component).OrderBy(x => x.GetType(), new SimpleTypeComparer()).ToArray();
        var types = comps.Select(x => x.GetType()).ToArray();
        this.InternalComponents.Clear();

        int compId = -1;
        for (int i = 0; i < comps.Length; i++)
        {
            comps[i].Id = i;
            this.InternalComponents.Add(comps[i]);
            if (this.InternalComponents[i].GetType() == component.GetType())
            {
                compId = i;
            }
        }

        this.ComponentTypes = new Memory<Type>(types);
        return new ComponentProxy<TComponent>(UpdateComponent, (TComponent)this.InternalComponents[compId]);
    }

    /// <inheritdoc />
    public void RemoveComponent<TComponent>() where TComponent : IComponent
    {
        var comps = this.Components.Where(x => x.GetType() != typeof(TComponent)).ToArray();
        var types = comps.Select(x => x.GetType()).ToArray();
        this.InternalComponents.Clear();

        for (int i = 0; i < comps.Length; i++)
        {
            comps[i].Id = i;
            this.InternalComponents.Add(comps[i]);
        }

        this.ComponentTypes = new Memory<Type>(types);
    }
}
