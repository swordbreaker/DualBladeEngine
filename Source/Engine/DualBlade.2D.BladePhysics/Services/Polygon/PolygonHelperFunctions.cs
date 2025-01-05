using DualBlade._2D.BladePhysics.Models;

namespace DualBlade._2D.BladePhysics.Services.Polygon;

public static class PolygonHelperFunctions
{
    public static Vector2 GetCollisionPoint(PolygonCollider polygonA, PolygonCollider polygonB, Vector2 minAxis, float overlap)
    {
        // Find the edge of polygonA that is most aligned with the minAxis
        int edgeIndexA = GetMostAlignedEdge(polygonA, minAxis);
        var edgeA = polygonA.AbsoluteVertices[(edgeIndexA + 1) % polygonA.AbsoluteVertices.Length] - polygonA.AbsoluteVertices[edgeIndexA];

        // Find the vertex of polygonB that is deepest along -minAxis
        int deepestVertexIndex = GetDeepestVertex(polygonB, -minAxis);
        var deepestVertex = polygonB.AbsoluteVertices[deepestVertexIndex];

        // Project the deepest vertex onto the edge of polygonA
        float t = Vector2.Dot(deepestVertex - polygonA.AbsoluteVertices[edgeIndexA], edgeA) / edgeA.LengthSquared();
        t = Math.Clamp(t, 0, 1); // Ensure the point is on the edge

        var collisionPoint = polygonA.AbsoluteVertices[edgeIndexA] + edgeA * t;

        // Move the collision point back along the minAxis by half the overlap
        collisionPoint -= minAxis * (overlap * 0.5f);

        return collisionPoint;
    }

    public static (float Min, float Max) Project(PolygonCollider polygon, Vector2 axis)
    {
        float min = float.MaxValue;
        float max = float.MinValue;

        foreach (var vertex in polygon.AbsoluteVertices)
        {
            float projection = Vector2.Dot(vertex, axis);
            min = Math.Min(min, projection);
            max = Math.Max(max, projection);
        }

        return (min, max);
    }

    public static bool Overlaps(this (float Min, float Max) a, (float Min, float Max) b)
    {
        return a.Max >= b.Min && b.Max >= a.Min;
    }

    public static float GetOverlap(this (float Min, float Max) a, (float Min, float Max) b)
    {
        return Math.Min(a.Max, b.Max) - Math.Max(a.Min, b.Min);
    }

    private static int GetMostAlignedEdge(PolygonCollider polygon, Vector2 axis)
    {
        int mostAlignedEdge = 0;
        float maxAlignment = float.MinValue;

        for (int i = 0; i < polygon.AbsoluteVertices.Length; i++)
        {
            Vector2 edge = polygon.AbsoluteVertices[(i + 1) % polygon.AbsoluteVertices.Length] - polygon.AbsoluteVertices[i];
            float alignment = Math.Abs(Vector2.Dot(Vector2.Normalize(edge), axis));

            if (alignment > maxAlignment)
            {
                maxAlignment = alignment;
                mostAlignedEdge = i;
            }
        }

        return mostAlignedEdge;
    }

    private static int GetDeepestVertex(PolygonCollider polygon, Vector2 axis)
    {
        int deepestVertex = 0;
        float maxDepth = float.MinValue;

        for (int i = 0; i < polygon.AbsoluteVertices.Length; i++)
        {
            float depth = Vector2.Dot(polygon.AbsoluteVertices[i], axis);

            if (depth > maxDepth)
            {
                maxDepth = depth;
                deepestVertex = i;
            }
        }

        return deepestVertex;
    }
}
