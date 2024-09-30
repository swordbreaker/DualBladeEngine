using DualBlade.Core.Entities;
using DualBlade.Core.Worlds;

namespace DualBlade.Core.Services;

public class EntityBuilder(IEntity current, Action<IEntity>? callback = null, EntityBuilder? parent = null)
{
    private delegate IEntity EntityOperation(IEntity entity, IWorld world);

    private List<EntityBuilder> children = new();

    private EntityOperation operation => (x, world) =>
    {
        var childEntities = new List<IEntity>();
        foreach (var child in children)
        {
            var c = child.AddToWorld(world);
            current.Children.Add(child.AddToWorld(world).Id);
            childEntities.Add(c);
        }

        var resultEntity = world.AddEntity(x);

        foreach (var childEntity in childEntities)
        {
            childEntity.Parent = resultEntity.Id;
            world.UpdateEntity(childEntity);
        }

        return resultEntity;
    };

    public EntityBuilder AddChild(IEntity entity)
    {
        var childBuilder = new EntityBuilder(entity, parent: this);
        children.Add(childBuilder);
        return childBuilder;
    }

    public EntityBuilder AddChildren(IEnumerable<EntityBuilder> entities)
    {
        children.AddRange(entities);
        return this;
    }

    public EntityBuilder BackToParent() =>
        parent ?? throw new InvalidOperationException("Cannot go back to parent this builder is the root");

    public IEntity AddToWorld(IWorld world)
    {
        var e = operation(current, world);
        callback?.Invoke(e);
        return e;
    }
}
