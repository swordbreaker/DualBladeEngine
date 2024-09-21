using FunctionalMonads.Monads.MaybeMonad;
using MonoGameEngine.Engine.Components;

namespace MonoGameEngine.Engine.Extensions;
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
}
