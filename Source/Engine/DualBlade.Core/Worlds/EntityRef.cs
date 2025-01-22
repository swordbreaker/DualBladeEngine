using DualBlade.Core.Entities;

namespace DualBlade.Core.Worlds;

/// <summary>
/// Reference to an entity.
/// Use <see cref="GetCopy"/> to get a copy of the entity (Readonly).
/// Use <see cref="GetProxy"/> to get a proxy to the entity.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class EntityRef<TEntity> where TEntity : IEntity
{
    private readonly int entityId;
    private readonly IWorld world;

    internal EntityRef(int entityId, IWorld world)
    {
        this.entityId = entityId;
        this.world = world;
    }

    /// <summary>
    /// Cast the reference to another entity type.
    /// </summary>
    /// <typeparam name="TOther"></typeparam>
    /// <returns></returns>
    public EntityRef<TOther> As<TOther>() where TOther : IEntity =>
        new(entityId, world);

    /// <summary>
    /// Gets a copy of the entity. This is a struct and will not update the original entity when modified.
    /// </summary>
    /// <returns></returns>
    public TEntity GetCopy() =>
        world.GetEntityProxy<TEntity>(entityId).Value;

    /// <summary>
    /// Gets a proxy to the entity. This will update the original entity when modified and then disposed.
    /// </summary>
    /// <returns></returns>
    public EntityProxy<TEntity> GetProxy() =>
        world.GetEntityProxy<TEntity>(entityId);
}