using System.Drawing;

namespace DualBlade._2D.BladePhysics.Models;

public interface ICollider
{
    object Tag { get; set; }

    Vector2 Offset { get; set; }
    Vector2 Scale { get; set; }

    Vector2 Center { get; set; }
    RectangleF Bounds { get; }

    bool IsTrigger { get; set; }

    bool IsStatic { get; set; }

    bool IsKinematic { get; set; }

    bool HitTest(ICollider collider, out CollisionInfo info);
}
