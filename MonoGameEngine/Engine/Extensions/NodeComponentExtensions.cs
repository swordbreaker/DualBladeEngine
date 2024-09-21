using FunctionalMonads.Monads.MaybeMonad;
using MonoGameEngine.Engine.Components;

namespace MonoGameEngine.Engine.Extensions;
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
}
