using DualBlade._2D.BladePhysics.Extensions;
using DualBlade._2D.BladePhysics.Models;
using Microsoft.VisualBasic;
using System.Threading.Tasks.Sources;
using System.Xml.Xsl;
using static DualBlade._2D.BladePhysics.Services.Polygon.PolygonHelperFunctions;

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

    public static bool HitTest(RectangleCollider rectangle, CircleCollider circle, out CollisionInfo info) =>
        HitTest(circle, rectangle, false, out info);

    public static bool HitTest(CircleCollider circle, RectangleCollider rectangle, out CollisionInfo info) =>
        HitTest(circle, rectangle, true, out info);

    private static bool HitTest(CircleCollider circle, RectangleCollider rectangle, bool isCirclePrimary,
        out CollisionInfo info)
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

    public static bool HitTest(PolygonCollider a, PolygonCollider b, out CollisionInfo info)
    {
        info = new CollisionInfo(a, b, Vector2.Zero, 0, Vector2.Zero);

        // Implement Separating Axis Theorem (SAT) for polygon-polygon collision
        var axes = a.Axes.Concat(b.Axes);

        float minOverlap = float.MaxValue;
        Vector2 minAxis = Vector2.Zero;

        foreach (var axis in axes)
        {
            var projectionA = Project(a, axis);
            var projectionB = Project(b, axis);

            if (!projectionA.Overlaps(projectionB))
            {
                return false;
            }

            float overlap = projectionA.GetOverlap(projectionB);
            if (overlap < minOverlap)
            {
                minOverlap = overlap;
                minAxis = axis;
            }
        }

        // Calculate collision normal and penetration depth
        var normal = minAxis;
        if (Vector2.Dot(b.Center - a.Center, normal) < 0)
        {
            normal = -normal;
        }

        var contactPoint = GetCollisionPoint(a, b, normal, minOverlap);

        info = new CollisionInfo(a, b, normal, minOverlap, contactPoint);
        return true;
    }

    public static bool HitTest(PolygonCollider a, RectangleCollider b, out CollisionInfo info)
    {
        var bounds = b.AbsoluteBounds();

        var polyB = new PolygonCollider(
        [
            new Vector2(bounds.Left, bounds.Top),
            new Vector2(bounds.Right, bounds.Top),
            new Vector2(bounds.Right, bounds.Bottom),
            new Vector2(bounds.Left, bounds.Bottom)
        ]);

        return HitTest(a, polyB, out info);
    }

    public static bool HitTest(RectangleCollider a, PolygonCollider b, out CollisionInfo info)
    {
        var bounds = a.AbsoluteBounds();
        var polyA = new PolygonCollider(
        [
            new Vector2(bounds.Left, bounds.Top),
            new Vector2(bounds.Right, bounds.Top),
            new Vector2(bounds.Right, bounds.Bottom),
            new Vector2(bounds.Left, bounds.Bottom)
        ]);

        return HitTest(polyA, b, out info);
    }

    public static bool HitTest(PolygonCollider a, CircleCollider b, out CollisionInfo info)
    {
        info = new CollisionInfo(a, b, Vector2.Zero, 0, Vector2.Zero);

        var circleCenter = b.Center + b.Offset;
        var radius = b.Radius * b.Scale.X;

        for (int i = 0; i < a.AbsoluteVertices.Length; i++)
        {
            var vertexA = a.AbsoluteVertices[i];
            var vertexB = a.AbsoluteVertices[(i + 1) % a.AbsoluteVertices.Length];
            if (Physics.LineCircleCollision(vertexA, vertexB, circleCenter, radius, out var collisionPoint))
            {
                var direction = collisionPoint - circleCenter;
                var normal = Vector2.Normalize(direction);
                var distanceToCircleCenter = Vector2.Distance(collisionPoint, circleCenter);
                var penetrationDepth = radius - distanceToCircleCenter;
                var contactPoint = collisionPoint;
                info = new CollisionInfo(b, a, normal, penetrationDepth, contactPoint);
                return true;
            }
        }

        return false;
    }

    public static bool HitTest(CircleCollider a, PolygonCollider b, out CollisionInfo info)
    {
        var result = HitTest(b, a, out info);
        info.Collider = a;
        info.OtherCollider = b;
        info.Normal = -info.Normal;
        return result;
    }
}