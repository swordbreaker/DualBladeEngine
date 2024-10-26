using DualBlade._2D.BladePhysics.Extensions;
using DualBlade.Core.Extensions;
using System.Drawing;

namespace DualBlade._2D.BladePhysics.Models;

public struct CircleCollider : ICollider
{
    public CircleCollider()
    {
    }

    public CircleCollider(Vector2 center, float radius)
    {
        Center = center;
        Radius = radius;
    }

    public float Radius { get; set; } = 1f;
    public Vector2 Center { get; set; } = Vector2.Zero;

    public readonly RectangleF Bounds => new(Center.X - Radius, Center.Y - Radius, Radius * 2, Radius * 2);
    public bool IsTrigger { get; set; } = false;
    public bool IsStatic { get; set; } = false;
    public bool IsKinematic { get; set; } = true;
    public Vector2 AbsolutePosition { get; set; }
    public Vector2 Offset { get; set; }
    public Vector2 Scale { get; set; }

    public readonly bool HitTest(ICollider collider, out CollisionInfo info)
    {
        info = default;
        return collider switch
        {
            CircleCollider other => HitTest(other, out info),
            RectangleCollider other => HitTest(other, out info),
            _ => false
        };
    }

    private readonly bool HitTest(CircleCollider collider, out CollisionInfo info)
    {
        var thisCenter = Center + Offset;
        var thisRadius = Radius * Scale.X;
        var otherCenter = collider.Center + collider.Offset;
        var otherRadius = collider.Radius * collider.Scale.X;

        var distance = Vector2.Distance(thisCenter, otherCenter);
        var radiusSum = thisRadius + otherRadius;

        var normal = Vector2.Normalize(otherCenter - this.Center);
        var penetrationDepth = radiusSum - distance;
        var contactPoint = Center + normal * thisRadius;

        info = new CollisionInfo
        {
            Collider = this,
            OtherCollider = collider,
            Normal = normal,
            PenetrationDepth = penetrationDepth,
            ContactPoint = contactPoint
        };

        return distance <= radiusSum;
    }

    private readonly bool HitTest(RectangleCollider collider, out CollisionInfo info)
    {
        var thisCenter = Center + Offset;
        var thisRadius = Radius * Scale.X;
        var otherCenter = collider.Center + collider.Offset;
        var otherSize = collider.Size * collider.Scale;

        var rectBounds = collider.AbsoluteBounds();

        var closestX = Math.Max(rectBounds.Left, Math.Min(Center.X, rectBounds.Right));
        var closestY = Math.Max(rectBounds.Top, Math.Min(Center.Y, rectBounds.Bottom));
        float distanceX = Center.X - closestX;
        float distanceY = Center.Y - closestY;
        var distanceSquared = distanceX * distanceX + distanceY * distanceY;

        var normal = Vector2.Zero;
        if (distanceSquared > 0)
        {
            normal = Vector2.Normalize(new Vector2(distanceX, distanceY));
        }
        else
        {
            // Circle center is inside the rectangle, use the closest edge
            var edgeDistX = MathF.Min(MathF.Abs(thisCenter.X - rectBounds.X), MathF.Abs(thisCenter.X - (rectBounds.X + rectBounds.Width)));
            var edgeDistY = MathF.Min(MathF.Abs(thisCenter.Y - rectBounds.Y), MathF.Abs(thisCenter.Y - (rectBounds.Y + rectBounds.Height)));

            if (edgeDistX < edgeDistY)
            {
                normal = new Vector2(thisCenter.X < rectBounds.X + rectBounds.Width / 2 ? -1 : 1, 0);
            }
            else
            {
                normal = new Vector2(0, thisCenter.Y < rectBounds.Y + rectBounds.Height / 2 ? -1 : 1);
            }
        }

        var penetrationDepth = thisRadius - MathF.Sqrt(distanceSquared);
        var contactPoint = thisCenter * thisRadius;

        info = new CollisionInfo
        {
            Collider = this,
            OtherCollider = collider,
            Normal = normal,
            PenetrationDepth = penetrationDepth,
            ContactPoint = contactPoint
        };

        return distanceSquared < thisRadius * thisRadius;
    }
}