using DualBlade._2D.BladePhysics.Models;
using DualBlade.Core.Extensions;

namespace DualBlade._2D.BladePhysics.Services;
public class UniformGrid
{
    private List<ICollider>[,] grid = new List<ICollider>[0, 0];
    private float cellSize;
    private int rows, cols;

    public UniformGrid(float cellSize, float width, float height)
    {
        this.cellSize = cellSize;
        rows = (int)Math.Ceiling(height / cellSize);
        cols = (int)Math.Ceiling(width / cellSize);
        grid = new List<ICollider>[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                grid[i, j] = [];
            }
        }
    }

    public void Insert(ICollider collider, Vector2 position)
    {
        var bounds = collider.Bounds;
        bounds.Offset(position.ToPointF());

        int minX = (int)(bounds.X / cellSize);
        int minY = (int)(bounds.Y / cellSize);
        int maxX = (int)((bounds.X + bounds.Width) / cellSize);
        int maxY = (int)((bounds.Y + bounds.Height) / cellSize);

        for (int y = minY; y <= maxY; y++)
        {
            for (int x = minX; x <= maxX; x++)
            {
                if (x >= 0 && x < cols && y >= 0 && y < rows)
                {
                    grid[y, x].Add(collider);
                }
            }
        }
    }

    public void Remove(ICollider collider, Vector2 position)
    {
        var bounds = collider.Bounds;
        bounds.Offset(position.ToPointF());

        int minX = (int)(bounds.X / cellSize);
        int minY = (int)(bounds.Y / cellSize);
        int maxX = (int)((bounds.X + bounds.Width) / cellSize);
        int maxY = (int)((bounds.Y + bounds.Height) / cellSize);

        for (int y = minY; y <= maxY; y++)
        {
            for (int x = minX; x <= maxX; x++)
            {
                if (x >= 0 && x < cols && y >= 0 && y < rows)
                {
                    grid[y, x].Remove(collider);
                }
            }
        }
    }

    public void Update(ICollider collider, Vector2 oldPos, Vector2 newPos)
    {
        var bounds = collider.Bounds;
        bounds.Offset(oldPos.ToPointF());

        int oldMinX = (int)(bounds.X / cellSize);
        int oldMinY = (int)(bounds.Y / cellSize);
        int oldMaxX = (int)((bounds.X + bounds.Width) / cellSize);
        int oldMaxY = (int)((bounds.Y + bounds.Height) / cellSize);

        int newMinX = (int)(newPos.X / cellSize);
        int newMinY = (int)(newPos.Y / cellSize);
        int newMaxX = (int)((newPos.X + bounds.Width) / cellSize);
        int newMaxY = (int)((newPos.Y + bounds.Height) / cellSize);

        if (oldMinX != newMinX || oldMinY != newMinY || oldMaxX != newMaxX || oldMaxY != newMaxY)
        {
            Remove(collider, oldPos);
            Insert(collider, newPos);
        }
    }

    public IEnumerable<ICollider> Query(ICollider collider)
    {
        var bounds = collider.Bounds;
        return Query(bounds.X, bounds.Y, bounds.Width, bounds.Height);
    }

    public IEnumerable<ICollider> Query(float x, float y, float width, float height)
    {
        HashSet<ICollider> result = [];

        int minX = (int)(x / cellSize);
        int minY = (int)(y / cellSize);
        int maxX = (int)((x + width) / cellSize);
        int maxY = (int)((y + height) / cellSize);

        for (int iy = minY; iy <= maxY; iy++)
        {
            for (int ix = minX; ix <= maxX; x++)
            {
                if (ix >= 0 && ix < cols && iy >= 0 && iy < rows)
                {
                    result.UnionWith(grid[iy, ix]);
                }
            }
        }

        return result;
    }

    public void Clear()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                grid[i, j].Clear();
            }
        }
    }
}
