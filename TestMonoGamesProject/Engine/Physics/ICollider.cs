using Microsoft.Xna.Framework;
using System.Drawing;

namespace TestMonoGamesProject.Engine.Physics
{
    public interface ICollider
    {
        RectangleF BoundingBox { get; }

        bool IntersectsWith(ICollider other);

        ICollider SetLocation(Vector2 pos);
    }
}
