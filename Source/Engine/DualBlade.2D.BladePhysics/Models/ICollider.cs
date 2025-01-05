using System.Drawing;

namespace DualBlade._2D.BladePhysics.Models;

public interface ICollider
{
    Guid Id { get; }

    object Tag { get; set; }

    Vector2 Offset { get; }
    Vector2 Scale { get; }
    float Rotation { get; }

    Vector2 Center { get; set; }
    RectangleF Bounds { get; }

    bool IsTrigger { get; set; }

    bool HitTest(ICollider collider, out CollisionInfo info);

    bool Update(Vector2 offset, Vector2 scale, float rotation);
}

public interface IColliderWithAbsoluteBounds : ICollider
{
    RectangleF AbsoluteBounds { get; }
}