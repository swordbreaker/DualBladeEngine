using DualBlade._2D.BladePhysics.Extensions;
using System.Drawing;
using DualBlade._2D.BladePhysics.Services;

namespace DualBlade._2D.BladePhysics.Models;

public struct CircleCollider : ICollider
{
    public CircleCollider()
    {
    }

    public CircleCollider(Vector2 center, float radius) : this()
    {
        Center = center;
        Radius = radius;
    }

    public Guid Id { get; } = Guid.NewGuid();

    public float Radius { get; set; } = 1f;
    public Vector2 Center { get; set; } = Vector2.Zero;

    public readonly RectangleF Bounds => new(Center.X - Radius, Center.Y - Radius, Radius * 2, Radius * 2);
    public bool IsTrigger { get; set; } = false;
    public bool IsStatic { get; set; } = false;
    public bool IsKinematic { get; set; } = true;
    public Vector2 Offset { get; set; }
    public Vector2 Scale { get; set; } = Vector2.One;
    public object Tag { get; set; }

    public readonly bool HitTest(ICollider collider, out CollisionInfo info)
    {
        info = default;
        return collider switch
        {
            CircleCollider other => ColliderHitTestCalculations.HitTest(this, other, out info),
            RectangleCollider other => ColliderHitTestCalculations.HitTest(this, other, out info),
            _ => false
        };
    }
}