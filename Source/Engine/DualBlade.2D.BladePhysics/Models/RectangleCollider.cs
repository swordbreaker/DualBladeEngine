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
    public Vector2 Offset { get; private set; } = Vector2.Zero;
    public Vector2 Scale { get; private set; } = Vector2.One;
    public float Rotation { get; } = 0f;

    public object Tag { get; set; } = new object();

    public readonly bool HitTest(ICollider collider, out CollisionInfo info)
    {
        info = default;
        return collider switch
        {
            RectangleCollider other => ColliderHitTestCalculations.HitTest(this, other, out info),
            CircleCollider other => ColliderHitTestCalculations.HitTest(this, other, out info),
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

        if (rotation != 0f)
        {
            throw new InvalidOperationException("A Rectangle collider cannot be rotate use a Polygon collider instead.");
        }

        return offsetChanged || scaleChanged || rotationChanged;
    }
}