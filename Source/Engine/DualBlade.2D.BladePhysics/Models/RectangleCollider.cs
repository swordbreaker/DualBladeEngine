using System.Drawing;

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
            RectangleCollider other => HitTest(other, out info),
            CircleCollider other => HitTest(other, out info),
            _ => false
        };
    }

    private readonly bool HitTest(RectangleCollider collider, out CollisionInfo info)
    {
        info = default;

        var thisCenter = Center + Offset;
        var otherCenter = collider.Center + collider.Offset;
        var thisSize = Size * Scale;
        var otherSize = collider.Size * collider.Scale;

        var dx = thisCenter.X - otherCenter.X;
        var dy = thisCenter.Y - otherCenter.Y;
        var halfWidth = (thisSize.X + otherSize.X) / 2;
        var halfHeight = (thisSize.Y + otherSize.Y) / 2;

        if (dx > halfWidth || dy > halfHeight)
        {
            return false;
        }

        var overlapX = halfWidth - Math.Abs(dx);
        var overlapY = halfHeight - Math.Abs(dy);

        if (overlapX < overlapY)
        {
            var normalX = dx < 0 ? -1 : 1;
            info.Normal = new Vector2(normalX, 0);
            info.PenetrationDepth = overlapX;
        }
        else
        {
            var normalY = dy < 0 ? -1 : 1;
            info.Normal = new Vector2(0, normalY);
            info.PenetrationDepth = overlapY;
        }

        info.Collider = this;
        info.OtherCollider = collider;
        info.ContactPoint = thisCenter + info.Normal * thisSize / 2;

        return true;
    }

    private readonly bool HitTest(CircleCollider collider, out CollisionInfo info) => collider.HitTest(this, out info);
}