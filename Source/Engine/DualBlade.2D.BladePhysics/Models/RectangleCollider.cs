using System.Drawing;
using DualBlade._2D.BladePhysics.Services;

namespace DualBlade._2D.BladePhysics.Models;

public struct RectangleCollider : ICollider
{
    public RectangleCollider()
    {
    }

    public RectangleCollider(Vector2 center, Vector2 size)
    {
        Center = center;
        Size = size;
    }

    public Guid Id { get; } = Guid.NewGuid();

    public Vector2 Center { get; set; } = Vector2.Zero;

    public Vector2 Size { get; set; } = Vector2.One;

    public readonly RectangleF Bounds => new(Center.X - Size.X / 2, Center.Y - Size.Y / 2, Size.X, Size.Y);
    public bool IsTrigger { get; set; } = false;
    public bool IsStatic { get; set; } = false;
    public bool IsKinematic { get; set; } = true;
    public Vector2 Offset { get; set; } = Vector2.Zero;
    public Vector2 Scale { get; set; } = Vector2.One;
    public object Tag { get; set; } = new object();

    public readonly bool HitTest(ICollider collider, out CollisionInfo info)
    {
        info = default;
        return collider switch
        {
            RectangleCollider other => ColliderHitTestCalculations.HitTest(this, other, out info),
            CircleCollider other => ColliderHitTestCalculations.HitTest(this, other, out info),
            _ => false
        };
    }
}