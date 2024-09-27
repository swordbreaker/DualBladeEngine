namespace DualBlade.Core.Worlds;
public class EntityHierarchyBuilder(TestWorld world)
{
    public EntityBuilder<TEntity> Add<TEntity>(TEntity entity) where TEntity : ITestEntity
    {
        return new EntityBuilder<TEntity>();
    }
}


public class EntityBuilder<TEntity>
{
    public EntityBuilder<TChild> AddChild<TChild>(TChild child)
    {
        return new EntityBuilder<TChild>();
    }
}