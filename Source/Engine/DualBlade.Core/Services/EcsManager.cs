using DualBlade.Core.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Worlds;

namespace DualBlade.Core.Services;

public class EcsManager(IWorld world) : IEcsManager
{
    public EntityProxy<IEntity> GetEntity(IComponent component)
    {
        return world.GetEntityProxy<IEntity>(component.EntityId);
    }

    public void DestroyEntity(IEntity entity)
    {
        world.Destroy(entity);
    }

    public void AddChild(IEntity parent, IEntity child)
    {
        parent.Children.Add(child.Id);
        child.Parent = parent.Id;
    }

    public void AddParent(IEntity child, IEntity parent)
    {
        if (child.Parent >= 0)
        {
            using var proxy = world.GetEntityProxy<IEntity>(child.Parent);
            proxy.Value.Children.Remove(child.Id);
        }

        child.Parent = parent.Id;
        parent.Children.Add(child.Id);
    }

    public EntityProxy<IEntity> GetParent(IEntity entity) => world.GetEntityProxy<IEntity>(entity.Parent);

    public IEnumerable<EntityRef<IEntity>> GetChildren(IEntity entity) =>
        entity.Children.ToSpan().ToArray().Select(world.GetEntityRef<IEntity>);

    public void UpdateComponent(IEntity entity, IComponent component)
    {
        var e = world.GetEntityRef<IEntity>(entity.Id).GetCopy();
        e.UpdateComponent(component);
        world.UpdateEntity(e);
    }

    public void UpdateEntity(IEntity entity)
    {
        world.UpdateEntity(entity);
    }

    public void TraverseToParent(IEntity node, Action<IEntity> action)
    {
        action(node);
        if (node.Parent >= 0)
        {
            using var p = world.GetEntityProxy<IEntity>(node.Parent);
            TraverseToParent(p.Value, action);
        }
    }
}