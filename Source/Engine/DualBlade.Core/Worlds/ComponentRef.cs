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
    private readonly int entityId;
    private readonly int id;
    private readonly IWorld world;

    internal ComponentRef(int entityId, int id, IWorld world)
    {
        this.entityId = entityId;
        this.id = id;
        this.world = world;
    }

    public ComponentRef<TOther> As<TOther>() where TOther : IComponent =>
        new(entityId, id, world);

    /// <summary>
    /// Gets a copy of the component.
    /// </summary>s
    public T GetCopy() =>
        world.GetEntityProxy<IEntity>(entityId).Value.Component<T>();

    /// <summary>
    /// Gets a proxy for the component that allows for mutation.
    /// </summary>
    public ComponentProxy<T> GetProxy()
    {
        var eProxy = world.GetEntityProxy<IEntity>(entityId);
        return new ComponentProxy<T>(eProxy.Value.UpdateComponent, eProxy.Value.Component<T>());
    }
}
