using FunctionalMonads.Monads.MaybeMonad;
using Microsoft.Xna.Framework;
using TestMonoGamesProject.Engine.Components;

namespace TestMonoGamesProject.Engine.Extensions;
public static class TransformComponentExtensions
{
    public static Vector2 AbsolutePosition(this TransformComponent transform)
    {
        var position = transform.Position;
        var parent = transform.Parent;
        while (parent is Some<TransformComponent> someParent)
        {
            position += someParent.Value.Position;
            parent = someParent.Value.Parent;

            position += someParent.Value.Position;
        }

        return position;
    }

    public static Vector2 AbsoluteScale(this TransformComponent transform)
    {
        var scale = transform.Scale;
        var parent = transform.Parent;
        while (parent is Some<TransformComponent> someParent)
        {
            scale *= someParent.Value.Scale;
            parent = someParent.Value.Parent;
        }

        return scale;
    }

    public static float AbsoluteRotation(this TransformComponent transform)
    {
        var rotation = transform.Rotation;
        var parent = transform.Parent;
        while (parent is Some<TransformComponent> someParent)
        {
            rotation += someParent.Value.Rotation;
            parent = someParent.Value.Parent;
        }

        return rotation;
    }

    public static void AddChild(this TransformComponent parent, TransformComponent child)
    {
        child.Parent = Maybe.Some(parent);
        parent.Children.Add(child);
    }

    public static void AddParent(this TransformComponent child, TransformComponent parent)
    {
        child.Parent = Maybe.Some(parent);
        parent.Children.Add(child);
    }
}
