using DualBlade.Core.Components;

namespace DualBlade.Core.Extensions;

public static class NodeComponentExtensions
{
    public static void AddChild<TParent, TChild>(this TParent parent, TChild child)
        where TParent : INodeComponent, IInternalComponent where TChild : INodeComponent, IInternalComponent
    {
        child.Parent = parent.EntityId;
        parent.Children.Add(child.EntityId);
    }

    public static void AddParent<TParent, TChild>(this TChild child, TParent parent)
        where TParent : INodeComponent, IInternalComponent where TChild : INodeComponent, IInternalComponent
    {
        child.Parent = parent.EntityId;
        parent.Children.Add(child.EntityId);
    }

    public static void TraverseToParent(this INodeComponent node, Action<INodeComponent> action)
    {
        action(node);
        //node.Parent?.TraverseToParent(action);
    }
}
