﻿using DualBlade.Core.Components;

namespace DualBlade.Core.Worlds;

/// <summary>
/// Proxy for a component that allows for mutation.
/// This is a ref struct to avoid heap allocations.
/// This copies the value and stores it for manipulation, and then updates the original value when disposed.
/// 
/// You cannot store this in a field or return it from a method.
/// Only use this within a method scope.
/// </summary>
public unsafe ref struct ComponentProxy<T> where T : IComponent
{
    private readonly int id;
    private readonly Action<IComponent> updateAction;
    private T value;

    internal ComponentProxy(Action<IComponent> updateAction, T value, int id)
    {
        this.updateAction = updateAction;
        this.id = id;
        this.value = value;
    }

    public readonly ComponentProxy<TOther> As<TOther>() where TOther : T =>
        new(updateAction, (TOther)value, id);

    public ref T Value => ref value;

    public readonly void Dispose() => updateAction(value);
}
