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
    public Vector2 Offset { get; set; }
    public Vector2 Scale { get; set; } = Vector2.One;
    public float Rotation { get; private set; } = 0f;
    public object Tag { get; set; } = new object();

    public readonly bool HitTest(ICollider collider, out CollisionInfo info)
    {
        info = default;
        return collider switch
        {
            CircleCollider other => ColliderHitTestCalculations.HitTest(this, other, out info),
            RectangleCollider other => ColliderHitTestCalculations.HitTest(this, other, out info),
            PolygonCollider other => ColliderHitTestCalculations.HitTest(this, other, out info),
            _ => false
        };
    }

    public bool Update(Vector2 offset, Vector2 scale, float rotation)
    {
        var offsetChanged = offset != this.Offset;
        var scaleChanged = scale != this.Scale;
        var rotationChanged = rotation != this.Rotation;

        this.Offset = offset;
        this.Scale = scale;
        this.Rotation = rotation;

        return offsetChanged || scaleChanged || rotationChanged;
    }
}