using DualBlade._2D.BladePhysics.Services;
using System.Drawing;

namespace DualBlade._2D.BladePhysics.Models;

public struct PolygonCollider : IColliderWithAbsoluteBounds
{
    public Guid Id { get; } = Guid.NewGuid();
    public object Tag { get; set; } = new object();
    public Vector2 Offset { readonly get; private set; } = Vector2.Zero;
    public Vector2 Scale { readonly get; private set; } = Vector2.One;
    public float Rotation { readonly get; private set; } = 0f;

    public Vector2 Center { get; set; } = Vector2.Zero;
    public RectangleF Bounds { get; private set; }
    public bool IsTrigger { get; set; }

    public Vector2[] Vertices { get; private set; }

    public Vector2[] AbsoluteVertices { get; private set; } = [];

    public Vector2[] Axes { get; private set; } = [];
    public RectangleF AbsoluteBounds { get; private set; }

    public PolygonCollider(Vector2[] points)
    {
        Vertices = points;
        OnVerticesChanged();
    }

    private void OnVerticesChanged()
    {
        UpdateAbsoluteVertices();
        UpdateBounds();
        UpdateAbsoluteBounds();
        UpdateAxes();
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

    private void UpdateAbsoluteBounds()
    {
        float minX = float.MaxValue, minY = float.MaxValue;
        float maxX = float.MinValue, maxY = float.MinValue;

        foreach (var point in AbsoluteVertices)
        {
            minX = Math.Min(minX, point.X);
            minY = Math.Min(minY, point.Y);
            maxX = Math.Max(maxX, point.X);
            maxY = Math.Max(maxY, point.Y);
        }

        AbsoluteBounds = new RectangleF(minX, minY, maxX - minX, maxY - minY);
    }

    private readonly Matrix TransformMatrix =>
        Matrix.CreateTranslation(Offset.X, Offset.Y, 0) *
        Matrix.CreateScale(Scale.X, Scale.Y, 1) *
        Matrix.CreateRotationZ(-MathHelper.ToRadians(Rotation));

    private void UpdateAxes()
    {
        Axes = GetAxes(this).ToArray();
    }

    private void UpdateAbsoluteVertices()
    {
        var m = TransformMatrix;
        AbsoluteVertices = Vertices.Select(v => Vector2.Transform(v, m)).ToArray();
    }

    public readonly bool HitTest(ICollider collider, out CollisionInfo info) =>
        collider switch
        {
            CircleCollider circle => ColliderHitTestCalculations.HitTest(this, circle, out info),
            RectangleCollider rectangle => ColliderHitTestCalculations.HitTest(this, rectangle, out info),
            PolygonCollider polygon => ColliderHitTestCalculations.HitTest(this, polygon, out info),
            _ => throw new NotImplementedException()
        };

    private static IEnumerable<Vector2> GetAxes(PolygonCollider polygon)
    {
        for (int i = 0; i < polygon.AbsoluteVertices.Length; i++)
        {
            var edge = polygon.AbsoluteVertices[(i + 1) % polygon.AbsoluteVertices.Length] - polygon.AbsoluteVertices[i];
            yield return Vector2.Normalize(new Vector2(-edge.Y, edge.X));
        }
    }

    public bool Update(Vector2 offset, Vector2 scale, float rotation)
    {
        var offsetChanged = offset != this.Offset;
        var scaleChanged = scale != this.Scale;
        var rotationChanged = rotation != this.Rotation;

        this.Offset = offset;
        this.Scale = scale;
        this.Rotation = rotation;

        var hasChanged = offsetChanged || scaleChanged || rotationChanged;

        if (hasChanged)
        {
            OnVerticesChanged();
        }

        return hasChanged;
    }
}