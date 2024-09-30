﻿using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;

namespace DualBlade._2D.Rendering.Extensions;
public static class TransformComponentExtensions
{
    public static Vector2 AbsolutePosition(this IEcsManager ecs, IEntity entity)
    {
        var position = Vector2.Zero;

        ecs.TraverseToParent(entity, e =>
        {
            if (e.TryGetComponent<TransformComponent>(out var parentTransform))
            {
                TransformComponent? parent = null;

                if (e.Parent >= 0)
                {
                    parent = ecs.GetParent(e).Value.Component<TransformComponent>().Value;
                }

                var m = (parent.HasValue)
                    ? Matrix.CreateScale(parent.Value.Scale.X, parent.Value.Scale.Y, 0) * Matrix.CreateRotationZ(parent.Value.Rotation)
                    : Matrix.Identity;

                position += Vector2.Transform(parentTransform.Value.Position, m);
            }
        });

        return position;
    }

    public static Vector2 AbsoluteScale(this IEcsManager ecs, IEntity entity)
    {
        var scale = Vector2.One;

        ecs.TraverseToParent(entity, e =>
        {
            if (e.TryGetComponent<TransformComponent>(out var parentTransform))
            {
                scale *= parentTransform.Value.Scale;
            }
        });

        return scale;
    }

    public static float AbsoluteRotation(this IEcsManager ecs, IEntity entity)
    {
        var rotation = 0f;

        ecs.TraverseToParent(entity, e =>
        {
            if (e.TryGetComponent<TransformComponent>(out var parentTransform))
            {
                rotation += parentTransform.Value.Rotation;
            }
        });

        return rotation;
    }
}
