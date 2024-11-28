using DualBlade._2D.BladePhysics.Extensions;
using DualBlade._2D.BladePhysics.Models;
using DualBlade.Core.Models;
using System.Drawing;

namespace DualBlade._2D.BladePhysics.Services;

public class UniformGrid
{
    private List<ICollider>[,] grid = new List<ICollider>[0, 0];

    private Dictionary<ICollider, (Vector2i min, Vector2i max)> colliderToMinMax = new();

    private readonly float cellSize;
    private readonly int rows;
    private readonly int cols;
    private readonly Vector2 offset;

    public UniformGrid(float cellSize, float width, float height)
    {
        this.cellSize = cellSize;
        offset = new Vector2(width, height) / 2;
        rows = (int)Math.Ceiling(height / cellSize);
        cols = (int)Math.Ceiling(width / cellSize);
        grid = new List<ICollider>[rows, cols];

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                grid[i, j] = [];
            }
        }
    }

    private (int minX, int minY, int maxX, int maxY) GetMinMax(RectangleF bounds)
    {
        var newBounds = bounds with { X = bounds.X + offset.X, Y = bounds.Y + offset.Y };

        var minX = (int)(newBounds.X / cellSize);
        var minY = (int)(newBounds.Y / cellSize);
        var maxX = (int)((newBounds.X + newBounds.Width) / cellSize);
        var maxY = (int)((newBounds.Y + newBounds.Height) / cellSize);

        return (minX, minY, maxX, maxY);
    }

    private void Foreach(ICollider collider, Action<List<ICollider>> action)
    {
        var (min, max) = colliderToMinMax[collider];

        for (var y = min.Y; y <= max.Y; y++)
        {
            for (var x = min.X; x <= max.X; x++)
            {
                if (x >= 0 && x < cols && y >= 0 && y < rows)
                {
                    action(grid[y, x]);
                }
            }
        }
    }

    public void Insert(ICollider collider)
    {
        var (minX, minY, maxX, maxY) = GetMinMax(collider.AbsoluteBounds());
        colliderToMinMax[collider] = (new Vector2(minX, minY), new Vector2(maxX, maxY));

        Foreach(collider, colliders => colliders.Add(collider));
    }

    public void Remove(ICollider collider)
    {
        Foreach(collider, colliders => colliders.Remove(collider));
        colliderToMinMax.Remove(collider);
    }

    public void Update(ICollider collider, Vector2 oldPos, Vector2 newPos)
    {
        var bounds = collider.AbsoluteBounds();
        var (oldMin, oldMax) = colliderToMinMax[collider];

        var newBound = bounds with { X = newPos.X, Y = newPos.Y };
        var (newMinX, newMinY, newMaxX, newMaxY) = GetMinMax(newBound);

        if (oldMin.X != newMinX || oldMin.Y != newMinY || oldMax.X != newMaxX || oldMax.Y != newMaxY)
        {
            Remove(collider);
            colliderToMinMax.Remove(collider);
            colliderToMinMax.Add(collider, (new(newMinX, newMinY), new(newMaxX, newMaxY)));

            Insert(collider);
        }
    }

    public IEnumerable<ICollider> Query(ICollider collider)
    {
        var (min, max) = colliderToMinMax[collider];
        return Query(min.X, min.Y, max.X, max.Y);
    }

    private IEnumerable<ICollider> Query(RectangleF bounds)
    {
        HashSet<ICollider> result = [];
        var (minX, minY, maxX, maxY) = GetMinMax(bounds);

        return Query(minX, minY, maxX, maxY);
    }

    private IEnumerable<ICollider> Query(int minX, int minY, int maxX, int maxY)
    {
        HashSet<ICollider> result = [];
        for (var y = minY; y <= maxY; y++)
        {
            for (var x = minX; x <= maxX; x++)
            {
                if (x >= 0 && x < cols && y >= 0 && y < rows)
                {
                    result.UnionWith(grid[y, x]);
                }
            }
        }

        return result;
    }

    public void Clear()
    {
        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                grid[i, j].Clear();
            }
        }
    }
}