using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Components;
using DualBlade.Core.Services;

namespace DualBlade._2D.Rendering.Extensions;
public static class TransformComponentExtensions
{
    public static Vector2 AbsolutePosition(this IEcsManager ecs, TransformComponent transform)
    {
        var position = Vector2.Zero;

        ecs.TraverseToParent(transform, p =>
        {
            if (p is TransformComponent parentTransform)
            {
                TransformComponent? parent = null;
                if (parentTransform.Parent.HasValue && parentTransform.Parent.Value.GetCopy() is TransformComponent transformComponent)
                {
                    parent = transformComponent;
                }

                var m = (parent.HasValue)
                    ? Matrix.CreateScale(parent.Value.Scale.X, parent.Value.Scale.Y, 0) * Matrix.CreateRotationZ(parent.Value.Rotation)
                    : Matrix.Identity;

                position += Vector2.Transform(parentTransform.Position, m);
            }
        });

        return position;
    }

    public static Vector2 AbsoluteScale(this IEcsManager ecs, TransformComponent transform)
    {
        var scale = Vector2.One;

        ecs.TraverseToParent(transform, p =>
        {
            if (p is TransformComponent parentTransform)
            {
                scale *= parentTransform.Scale;
            }
        });

        return scale;
    }

    public static float AbsoluteRotation(this IEcsManager ecs, TransformComponent transform)
    {
        var rotation = 0f;

        ecs.TraverseToParent(transform, p =>
        {
            if (p is TransformComponent parentTransform)
            {
                rotation += parentTransform.Rotation;
            }
        });

        return rotation;
    }
}
