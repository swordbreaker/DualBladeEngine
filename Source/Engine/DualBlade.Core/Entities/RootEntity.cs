using DualBlade.Core.Worlds;

namespace DualBlade.Core.Entities;

public class RootEntityConstructor
{
    private IEntity entity;
    private readonly IWorld world;

    private List<IEntity> children = new();

    public RootEntityConstructor(RootEntity rootEntity, IWorld world)
    {
        this.world = world;
        entity = world.AddEntity(rootEntity);
    }

    public void AddChildern(IEnumerable<IEntity> entities)
    {
        foreach (var entity in entities)
        {
            AddChild(entity);
        }
    }

    public void AddChild(IEntity entity)
    {
        var childEntity = world.AddEntity(entity);
        entity.Children.Add(childEntity.Id);
    }
}

public partial struct RootEntity : IEntity
{
}