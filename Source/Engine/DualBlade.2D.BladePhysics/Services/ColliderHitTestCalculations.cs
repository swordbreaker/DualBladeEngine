using DualBlade._2D.BladePhysics.Extensions;
using DualBlade._2D.BladePhysics.Models;

namespace DualBlade._2D.BladePhysics.Services;

public static class ColliderHitTestCalculations
{
    public static bool HitTest(CircleCollider circleA, CircleCollider circleB, out CollisionInfo info)
    {
        var aCenter = circleA.Center + circleA.Offset;
        var bCenter = circleB.Center + circleB.Offset;
        var aRadius = circleA.Radius * circleA.Scale.X;
        var bRadius = circleB.Radius * circleB.Scale.X;

        var distance = Vector2.Distance(aCenter, bCenter);
        var radiusSum = aRadius + bRadius;

        var direction = bCenter - aCenter;
        var normal = (direction.LengthSquared() > 0)
            ? Vector2.Normalize(bCenter - aCenter)
            : direction;

        var penetrationDepth = radiusSum - distance;
        var contactPoint = aCenter + normal * aRadius;

        info = new CollisionInfo
        {
            Collider = circleA,
            OtherCollider = circleB,
            Normal = normal,
            PenetrationDepth = penetrationDepth,
            ContactPoint = contactPoint
        };

        return distance <= radiusSum;
    }

    public static bool HitTest(CircleCollider circle, RectangleCollider rectangle, out CollisionInfo info)
    {
        var circleCenter = circle.Center + circle.Offset;
        var radius = circle.Radius * circle.Scale.X;

        var rectBounds = rectangle.AbsoluteBounds();

        var closestX = Math.Max(rectBounds.Left, Math.Min(circleCenter.X, rectBounds.Right));
        var closestY = Math.Max(rectBounds.Top, Math.Min(circleCenter.Y, rectBounds.Bottom));
        var distanceX = closestX - circleCenter.X;
        var distanceY = closestY - circleCenter.Y;
        var distance = MathF.Sqrt(distanceX * distanceX + distanceY * distanceY);

        var normal = Vector2.Normalize(new Vector2(distanceX, distanceY));
        var penetrationDepth = radius - distance;
        var contactPoint = circleCenter + new Vector2(distanceX, distanceY);

        info = new CollisionInfo
        {
            Collider = circle,
            OtherCollider = rectangle,
            Normal = normal,
            PenetrationDepth = penetrationDepth,
            ContactPoint = contactPoint
        };

        return distance < radius;
    }

    public static bool HitTest(RectangleCollider rectA, RectangleCollider rectB, out CollisionInfo info)
    {
        info = default;

        var centerA = rectA.Center + rectA.Offset;
        var centerB = rectB.Center + rectB.Offset;
        var sizeA = rectA.Size * rectA.Scale;
        var sizeB = rectB.Size * rectB.Scale;

        var dx = centerA.X - centerB.X;
        var dy = centerA.Y - centerB.Y;
        var halfWidth = (sizeA.X + sizeB.X) / 2;
        var halfHeight = (sizeA.Y + sizeB.Y) / 2;

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

        info.Collider = rectA;
        info.OtherCollider = rectB;
        info.ContactPoint = centerA + info.Normal * sizeA / 2;

        return true;
    }
}