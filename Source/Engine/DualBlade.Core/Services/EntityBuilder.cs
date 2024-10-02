using DualBlade.Core.Entities;
using DualBlade.Core.Worlds;

namespace DualBlade.Core.Services;

public class EntityBuilder
{
    private delegate IEntity EntityOperation(IEntity entity, IWorld world);

    private List<EntityBuilder> children = [];
    protected IEntity current;
    private readonly Action<IEntity>? callback;
    private readonly EntityBuilder? parent;

    public EntityBuilder(IEntity current, Action<IEntity>? callback = null, EntityBuilder? parent = null)
    {
        this.current = current;
        this.callback = callback;
        this.parent = parent;
    }

    private EntityOperation operation => (x, world) =>
    {
        var childEntities = new List<IEntity>();
        foreach (var child in children)
        {
            var c = child.AddToWorld(world);
            x.Children.Add(c.Id);
            childEntities.Add(c);
        }

        var resultEntity = world.AddEntity(x);

        foreach (var childEntity in childEntities)
        {
            childEntity.Parent = resultEntity.Id;
            world.UpdateEntity(childEntity);
            resultEntity.Children.Add(childEntity.Id);
        }

        return resultEntity;
    };

    public EntityBuilder<TEntity> AddChild<TEntity>(TEntity entity) where TEntity : IEntity
    {
        var childBuilder = new EntityBuilder<TEntity>(entity, parent: this);
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

public class EntityBuilder<TEntity>(TEntity entity, Action<IEntity>? callback = null, EntityBuilder? parent = null) : EntityBuilder(entity, callback, parent) where TEntity : IEntity
{
    public delegate void UpdateEntityAction(ref TEntity entity);

    public void UpdateEntity(UpdateEntityAction updateAction)
    {
        var e = (TEntity)this.current;
        updateAction(ref e);
        this.current = e;
    }
}
