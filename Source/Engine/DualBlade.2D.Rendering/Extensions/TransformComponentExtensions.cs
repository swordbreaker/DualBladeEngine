using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Extensions;
using FunctionalMonads.Monads.MaybeMonad;

namespace DualBlade._2D.Rendering.Extensions;
public static class TransformComponentExtensions
{
    public static Vector2 AbsolutePosition(this TransformComponent transform)
    {
        var position = Vector2.Zero;

        transform.TraverseToParent(p =>
        {
            if (p is TransformComponent parentTransform)
            {
                var parent = parentTransform.Parent as TransformComponent;

                var m = (parent is not null)
                    ? Matrix.CreateScale(parent.Scale.X, parent.Scale.Y, 0) * Matrix.CreateRotationZ(parent.Rotation)
                    : Matrix.Identity;

                position += Vector2.Transform(parentTransform.Position, m);
            }
        });

        return position;
    }

    public static Vector2 AbsoluteScale(this TransformComponent transform)
    {
        var scale = Vector2.One;

        transform.TraverseToParent(p =>
        {
            if (p is TransformComponent parentTransform)
            {
                scale *= parentTransform.Scale;
            }
        });

        return scale;
    }

    public static float AbsoluteRotation(this TransformComponent transform)
    {
        var rotation = 0f;

        transform.TraverseToParent(p =>
        {
            if (p is TransformComponent parentTransform)
            {
                rotation += parentTransform.Rotation;
            }
        });

        return rotation;
    }
}
