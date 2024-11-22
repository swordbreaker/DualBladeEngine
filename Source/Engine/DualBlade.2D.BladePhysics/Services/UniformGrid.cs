using DualBlade._2D.BladePhysics.Extensions;
using DualBlade._2D.BladePhysics.Models;
using DualBlade.Core.Models;
using System.Drawing;

namespace DualBlade._2D.BladePhysics.Services;

public class UniformGrid
{
    private List<ICollider>[,] grid = new List<ICollider>[0, 0];
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
        var newBounds = new RectangleF(bounds.X + offset.X, bounds.Y + offset.Y, bounds.Width, bounds.Height);

        var minX = (int)(newBounds.X / cellSize);
        var minY = (int)(newBounds.Y / cellSize);
        var maxX = (int)((newBounds.X + newBounds.Width) / cellSize);
        var maxY = (int)((newBounds.Y + newBounds.Height) / cellSize);

        return (minX, minY, maxX, maxY);
    }

    private void Foreach(RectangleF bounds, Action<List<ICollider>> action)
    {
        var (minX, minY, maxX, maxY) = GetMinMax(bounds);

        for (var y = minY; y <= maxY; y++)
        {
            for (var x = minX; x <= maxX; x++)
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
        Foreach(collider.AbsoluteBounds(), colliders => colliders.Add(collider));
    }

    public void Remove(ICollider collider)
    {
        Foreach(collider.AbsoluteBounds(), colliders => colliders.Remove(collider));
    }

    public void Update(ICollider collider, Vector2 oldPos, Vector2 newPos)
    {
        var bounds = collider.AbsoluteBounds();
        var (oldMinX, oldMinY, oldMaxX, oldMaxY) = GetMinMax(bounds);

        var newBound = new RectangleF(newPos.X, newPos.Y, bounds.Width, bounds.Height);
        var (newMinX, newMinY, newMaxX, newMaxY) = GetMinMax(newBound);

        if (oldMinX != newMinX || oldMinY != newMinY || oldMaxX != newMaxX || oldMaxY != newMaxY)
        {
            Remove(collider);
            Insert(collider);
        }
    }

    public IEnumerable<ICollider> Query(ICollider collider)
    {
        var bounds = collider.AbsoluteBounds();
        return Query(bounds.X, bounds.Y, bounds.Width, bounds.Height);
    }

    public IEnumerable<ICollider> Query(RectangleF bounds)
    {
        HashSet<ICollider> result = [];
        var (minX, minY, maxX, maxY) = GetMinMax(bounds);

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

    public IEnumerable<ICollider> Query(float x, float y, float width, float height)
    {
        var rect = new RectangleF(x, y, width, height);
        return Query(rect);
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