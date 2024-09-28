using DualBlade.Core.Components;

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
    private readonly IWorld world;
    private readonly int id;

    internal ComponentRef(IWorld world, int id)
    {
        this.world = world;
        this.id = id;
    }

    public ComponentRef<TOther> As<TOther>() where TOther : IComponent =>
        new(world, id);

    /// <summary>
    /// Gets a copy of the component.
    /// </summary>s
    public T GetCopy() =>
        world.GetComponentCopy<T>(id);

    /// <summary>
    /// Gets a proxy for the component that allows for mutation.
    /// </summary>
    public ComponentProxy<T> GetProxy() =>
        world.GetComponentProxy<T>(id);
}
