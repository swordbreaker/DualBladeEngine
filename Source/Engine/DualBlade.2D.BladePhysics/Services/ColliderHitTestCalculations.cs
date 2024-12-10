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

        var direction = aCenter - bCenter;
        var normal = (direction.LengthSquared() > 0)
            ? Vector2.Normalize(direction)
            : direction;

        var penetrationDepth = radiusSum - distance;
        var contactPoint = aCenter - normal * aRadius;

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

    public static bool HitTest(RectangleCollider rectangle, CircleCollider circle, out CollisionInfo info)
    {
        return HitTest(circle, rectangle, false, out info);
    }

    public static bool HitTest(CircleCollider circle, RectangleCollider rectangle, out CollisionInfo info)
    {
        return HitTest(circle, rectangle, true, out info);


        var circleCenter = circle.Center + circle.Offset;
        var radius = circle.Radius * circle.Scale.X;

        var rectBounds = rectangle.AbsoluteBounds();

        var closestX = Math.Max(rectBounds.Left, Math.Min(circleCenter.X, rectBounds.Right));
        var closestY = Math.Max(rectBounds.Top, Math.Min(circleCenter.Y, rectBounds.Bottom));
        var distanceX = circleCenter.X - closestX;
        var distanceY = circleCenter.Y - closestY;
        var distance = MathF.Sqrt(distanceX * distanceX + distanceY * distanceY);

        var normal = Vector2.Normalize(new Vector2(distanceX, distanceY));
        var penetrationDepth = radius - distance;
        var contactPoint = circleCenter - new Vector2(distanceX, distanceY);

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

    private static bool HitTest(CircleCollider circle, RectangleCollider rectangle, bool isCirclePrimary, out CollisionInfo info)
    {
        var circleCenter = circle.Center + circle.Offset;
        var radius = circle.Radius * circle.Scale.X;

        var rectBounds = rectangle.AbsoluteBounds();

        var closestX = Math.Max(rectBounds.Left, Math.Min(circleCenter.X, rectBounds.Right));
        var closestY = Math.Max(rectBounds.Top, Math.Min(circleCenter.Y, rectBounds.Bottom));
        var distanceX = circleCenter.X - closestX;
        var distanceY = circleCenter.Y - closestY;
        var distance = MathF.Sqrt(distanceX * distanceX + distanceY * distanceY);

        var normal = (isCirclePrimary)
            ? Vector2.Normalize(new Vector2(distanceX, distanceY))
            : Vector2.Normalize(new Vector2(-distanceX, -distanceY));

        var penetrationDepth = radius - distance;
        var contactPoint = new Vector2(closestX, closestY);

        info = new CollisionInfo
        {
            Collider = isCirclePrimary ? circle : rectangle,
            OtherCollider = isCirclePrimary ? rectangle : circle,
            Normal = normal,
            PenetrationDepth = penetrationDepth,
            ContactPoint = contactPoint
        };

        return distance < radius;
    }

    public static bool HitTest(RectangleCollider rectA, RectangleCollider rectB, out CollisionInfo info)
    {
        info = default;

        var boundsA = rectA.AbsoluteBounds();
        var boundsB = rectB.AbsoluteBounds();

        if (!boundsA.IntersectsWith(boundsB))
        {
            return false;
        }

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

        var direction = centerA - centerB;
        info.Normal = Vector2.Normalize(direction);
        info.PenetrationDepth = Math.Min(overlapX, overlapY);

        var contactX = info.Normal.X switch
        {
            > 0 => boundsA.Right,
            < 0 => boundsB.Left,
            _ => MathF.Max(boundsA.Left, boundsB.Left) + overlapX / 2,
        };

        var contactY = info.Normal.Y switch
        {
            > 0 => boundsA.Top,
            < 0 => boundsA.Bottom,
            _ => MathF.Max(boundsA.Top, boundsB.Top) + overlapY / 2,
        };

        info.Collider = rectA;
        info.OtherCollider = rectB;
        info.ContactPoint = new Vector2(contactX, contactY);

        return true;
    }
}