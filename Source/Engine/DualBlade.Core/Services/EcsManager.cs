using DualBlade.Core.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Worlds;

namespace DualBlade.Core.Services;

public interface IEcsManager
{
    EntityProxy<IEntity> GetEntity(IComponent component);
    void DestroyEntity(IEntity entity);
    void TraverseToParent(IEntity node, Action<IEntity> action);
    void AddParent<TParent, TChild>(IEntity child, IEntity parent);
    void AddChild<TParent, TChild>(IEntity parent, IEntity child);
    public EntityProxy<IEntity> GetParent(IEntity entity);
    public IEnumerable<IEntity> GetChildren(IEntity entity);

}

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

    public void AddChild<TParent, TChild>(IEntity parent, IEntity child)
    {
        parent.Children.Add(child.Id);
        child.Parent = parent.Id;
    }

    public void AddParent<TParent, TChild>(IEntity child, IEntity parent)
    {
        child.Parent = parent.Id;
        parent.Children.Add(child.Id);
    }

    public EntityProxy<IEntity> GetParent(IEntity entity) => world.GetEntityProxy<IEntity>(entity.Parent);

    public IEnumerable<IEntity> GetChildren(IEntity entity)
    {
        foreach (var child in entity.Children.ToSpan().ToArray())
        {
            yield return world.GetEntityProxy<IEntity>(child).Value;
        }
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
