using DualBlade.Core.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Worlds;

namespace DualBlade.Core.Services;

public interface IEcsManager
{
    EntityProxy<INodeEntity> GetEntity(IComponent component);
    void DestroyEntity(IComponent component);
    ComponentRef<TComponent>? GetAdjacentComponent<TComponent>(IComponent component) where TComponent : IComponent;
    void TraverseToParent(INodeComponent node, Action<INodeComponent> action);
    void AddParent<TParent, TChild>(INodeComponent child, INodeComponent parent);
    void AddChild<TParent, TChild>(INodeComponent parent, INodeComponent child);

}

public class EcsManager(IWorld world) : IEcsManager
{
    public EntityProxy<INodeEntity> GetEntity(IComponent component)
    {
        return world.GetEntityProxy<INodeEntity>(component.EntityId);
    }

    public void DestroyEntity(IComponent component)
    {
        var entity = GetEntity(component).Value;
        world.Destroy(entity);
    }

    public ComponentRef<TComponent>? GetAdjacentComponent<TComponent>(IComponent component) where TComponent : IComponent
    {
        var e = GetEntity(component);
        return e.Value.Component<TComponent>();
    }

    public void AddChild<TParent, TChild>(INodeComponent parent, INodeComponent child)
    {
        child.Parent = world.GetComponent<IComponent>(parent.Id);
        parent.Children.Add(world.GetComponent<IComponent>(child.Id));
    }

    public void AddParent<TParent, TChild>(INodeComponent child, INodeComponent parent)
    {
        child.Parent = world.GetComponent<IComponent>(parent.Id);
        parent.Children.Add(world.GetComponent<IComponent>(child.Id));
    }

    public void TraverseToParent(INodeComponent node, Action<INodeComponent> action)
    {
        action(node);
        if (node.Parent.HasValue && node.Parent.Value.GetCopy() is INodeComponent nodeComponent)
        {
            TraverseToParent(nodeComponent, action);
        }
    }

}
