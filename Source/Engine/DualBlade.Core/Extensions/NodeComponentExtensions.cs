using DualBlade.Core.Components;
using FunctionalMonads.Monads.MaybeMonad;

namespace DualBlade.Core.Extensions;

public static class NodeComponentExtensions
{
    public static void AddChild(this INodeComponent parent, INodeComponent child)
    {
        child.Parent = Maybe.Some(parent);
        parent.Children.Add(child);
    }

    public static void AddParent(this INodeComponent child, INodeComponent parent)
    {
        child.Parent = Maybe.Some(parent);
        parent.Children.Add(child);
    }

    public static void TraverseToParent(this INodeComponent node, Action<INodeComponent> action)
    {
        action(node);
        node.Parent.IfSome(p => p.TraverseToParent(action));
    }
}
