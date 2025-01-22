using DualBlade.Core.Components;

namespace DualBlade.Core.Worlds;

/// <summary>
/// Proxy for a component that allows for mutation.
/// This is a ref struct to avoid heap allocations.
/// This copies the value and stores it for manipulation, and then updates the original value when disposed.
/// 
/// You cannot store this in a field or return it from a method.
/// Only use this within a method scope.
/// </summary>
public unsafe ref struct ComponentProxy<T>(Action<IComponent> updateAction, T value) where T : IComponent
{
    private T value = value;

    public readonly ComponentProxy<TOther> As<TOther>() where TOther : T =>
        new(updateAction, (TOther)value);

    /// <summary>
    /// Gets the component.
    /// </summary>
    public ref T Value => ref value;

    /// <summary>
    /// Updates the original component.
    /// </summary>
    public readonly void Dispose() => updateAction(value);
}