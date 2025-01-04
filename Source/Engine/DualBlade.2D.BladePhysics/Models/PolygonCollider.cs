using System.Drawing;

namespace DualBlade._2D.BladePhysics.Models;
internal class PolygonCollider : ICollider
{
    public Guid Id { get; } = Guid.NewGuid();
    public object Tag { get; set; }
    public Vector2 Offset { get; set; }
    public Vector2 Scale { get; set; } = Vector2.One;
    public Vector2 Center { get; set; } = Vector2.Zero;
    public RectangleF Bounds { get; private set; }
    public bool IsTrigger { get; set; }

    public Vector2[] Vertices { get; private set; }

    public Vector2[] Axes { get; private set; }

    public PolygonCollider(Vector2[] points)
    {
        Vertices = points;
        UpdateBounds();
    }

    private void UpdateBounds()
    {
        float minX = float.MaxValue, minY = float.MaxValue;
        float maxX = float.MinValue, maxY = float.MinValue;

        foreach (var point in Vertices)
        {
            minX = Math.Min(minX, point.X);
            minY = Math.Min(minY, point.Y);
            maxX = Math.Max(maxX, point.X);
            maxY = Math.Max(maxY, point.Y);
        }

        Bounds = new RectangleF(minX, minY, maxX - minX, maxY - minY);
        Center = new Vector2((minX + maxX) / 2, (minY + maxY) / 2);
    }

    private void UpdateAxes()
    {
        Axes = GetAxes(this).ToArray();
    }

    public bool HitTest(ICollider collider, out CollisionInfo info) => throw new NotImplementedException();

    private static bool PolygonPolygonCollision(PolygonCollider a, PolygonCollider b, out CollisionInfo info)
    {
        info = new CollisionInfo(a, b, Vector2.Zero, 0, Vector2.Zero);

        // Implement Separating Axis Theorem (SAT) for polygon-polygon collision
        var axes = GetAxes(a).Concat(GetAxes(b));

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
        Vector2 normal = minAxis;
        if (Vector2.Dot(b.Center - a.Center, normal) < 0)
        {
            normal = -normal;
        }

        info = new CollisionInfo(a, b, normal, minOverlap, CalculateContactPoint(a, b, normal));
        return true;
    }

    private static IEnumerable<Vector2> GetAxes(PolygonCollider polygon)
    {
        for (int i = 0; i < polygon.Vertices.Length; i++)
        {
            var edge = polygon.Vertices[(i + 1) % polygon.Vertices.Length] - polygon.Vertices[i];
            yield return Vector2.Normalize(new Vector2(-edge.Y, edge.X));
        }
    }

    private static (float Min, float Max) Project(PolygonCollider polygon, Vector2 axis)
    {
        float min = float.MaxValue;
        float max = float.MinValue;

        foreach (var vertex in polygon.Vertices)
        {
            float projection = Vector2.Dot(vertex, axis);
            min = Math.Min(min, projection);
            max = Math.Max(max, projection);
        }

        return (min, max);
    }

    private static Vector2 CalculateContactPoint(PolygonCollider a, PolygonCollider b, Vector2 normal)
    {
        // Implement contact point calculation (e.g., using closest points or clipping)
        // This is a simplified version
        return (a.Center + b.Center) / 2;
    }
}

public static class ProjectionExtensions
{
    public static bool Overlaps(this (float Min, float Max) a, (float Min, float Max) b)
    {
        return a.Max >= b.Min && b.Max >= a.Min;
    }

    public static float GetOverlap(this (float Min, float Max) a, (float Min, float Max) b)
    {
        return Math.Min(a.Max, b.Max) - Math.Max(a.Min, b.Min);
    }
}