using DualBlade._2D.BladePhysics.Models;
using System.Drawing;

namespace DualBlade._2D.BladePhysics.Extensions;

public static class ColliderExtensions
{
    public static RectangleF AbsoluteBounds(this ICollider collider) =>
        new(collider.Bounds.X + collider.Offset.X, collider.Bounds.Y + collider.Offset.Y,
            collider.Bounds.Width * collider.Scale.X, collider.Bounds.Height * collider.Scale.Y);
}