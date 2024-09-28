using DualBlade.Core.Entities;

namespace DualBlade.Core.Worlds;

/// <summary>
/// Proxy for a entity that allows for mutation.
/// This is a ref struct to avoid heap allocations.
/// This copies the value and stores it for manipulation, and then updates the original value when disposed.
/// 
/// You cannot store this in a field or return it from a method.
/// Only use this within a method scope.
/// </summary>
public unsafe ref struct EntityProxy<T> where T : IEntity
{
    private readonly int id;
    private readonly Action<IEntity> updateAction;
    private T value;

    internal EntityProxy(Action<IEntity> updateAction, T value, int id)
    {
        this.updateAction = updateAction;
        this.id = id;
        this.value = value;
    }

    public readonly EntityProxy<TOther> As<TOther>() where TOther : T =>
        new(updateAction, (TOther)value, id);

    public ref T Value => ref value;

    public readonly void Dispose() => updateAction(value);
}
