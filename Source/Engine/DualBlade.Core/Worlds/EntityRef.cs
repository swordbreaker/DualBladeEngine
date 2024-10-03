using DualBlade.Core.Entities;

namespace DualBlade.Core.Worlds;
public class EntityRef<TEntity> where TEntity : IEntity
{
    private readonly int entityId;
    private readonly IWorld world;

    internal EntityRef(int entityId, IWorld world)
    {
        this.entityId = entityId;
        this.world = world;
    }

    public EntityRef<TOther> As<TOther>() where TOther : IEntity =>
        new(entityId, world);

    public TEntity GetCopy() =>
        world.GetEntityProxy<TEntity>(entityId).Value;

    public EntityProxy<TEntity> GetProxy() =>
        world.GetEntityProxy<TEntity>(entityId);
}
