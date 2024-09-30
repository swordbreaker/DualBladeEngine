using DualBlade.Core.Components;
using DualBlade.Core.Entities;

namespace DualBlade.Core.Worlds;

/// <summary>
/// Holds a reference to a component that allows for reading.
/// 
/// Use the <see cref="GetCopy"/> method to get a copy of the component.
/// Use the <see cref="GetProxy"/> method to get a proxy for the component that allows for mutation.
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly struct ComponentRef<T> where T : IComponent
{
    private readonly IEntity entity;
    private readonly int id;

    internal ComponentRef(IEntity entity, int id)
    {
        this.entity = entity;
        this.id = id;
    }

    public ComponentRef<TOther> As<TOther>() where TOther : IComponent =>
        new(entity, id);

    /// <summary>
    /// Gets a copy of the component.
    /// </summary>s
    public T GetCopy() =>
        entity.Component<T>().Value;

    /// <summary>
    /// Gets a proxy for the component that allows for mutation.
    /// </summary>
    public ComponentProxy<T> GetProxy() =>
        entity.Component<T>();
}
