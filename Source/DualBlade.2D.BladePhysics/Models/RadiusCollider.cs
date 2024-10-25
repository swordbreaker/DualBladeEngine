using System.Drawing;

namespace DualBlade._2D.BladePhysics.Models;

public struct RadiusCollider : ICollider
{
    public RadiusCollider()
    {
    }

    public RadiusCollider(Vector2 center, float radius)
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

    public readonly bool HitTest(ICollider collider, out CollisionInfo info)
    {
        info = default;
        return collider switch
        {
            RadiusCollider other => HitTest(other, out info),
            RectangleCollider other => HitTest(other, out info),
            _ => false
        };
    }

    private readonly bool HitTest(RadiusCollider collider, out CollisionInfo info)
    {
        var distance = Vector2.Distance(Center, collider.Center);
        var radiusSum = Radius + collider.Radius;

        var normal = Vector2.Normalize(collider.Center - this.Center);
        var penetrationDepth = radiusSum - distance;
        var contactPoint = Center + normal * Radius;

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
        var rectBounds = collider.Bounds;

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
            var edgeDistX = MathF.Min(MathF.Abs(Center.X - rectBounds.X), MathF.Abs(Center.X - (rectBounds.X + rectBounds.Width)));
            var edgeDistY = MathF.Min(MathF.Abs(Center.Y - rectBounds.Y), MathF.Abs(Center.Y - (rectBounds.Y + rectBounds.Height)));

            if (edgeDistX < edgeDistY)
            {
                normal = new Vector2(Center.X < rectBounds.X + rectBounds.Width / 2 ? -1 : 1, 0);
            }
            else
            {
                normal = new Vector2(0, Center.Y < rectBounds.Y + rectBounds.Height / 2 ? -1 : 1);
            }
        }

        var penetrationDepth = Radius - MathF.Sqrt(distanceSquared);
        var contactPoint = Center - normal * Radius;

        info = new CollisionInfo
        {
            Collider = this,
            OtherCollider = collider,
            Normal = normal,
            PenetrationDepth = penetrationDepth,
            ContactPoint = contactPoint
        };

        return distanceSquared < Radius * Radius;
    }
}