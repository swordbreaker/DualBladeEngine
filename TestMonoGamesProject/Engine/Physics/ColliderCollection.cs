using Microsoft.Xna.Framework;
using System;
using System.Drawing;
using System.Linq;

namespace TestMonoGamesProject.Engine.Physics
{
    public class ColliderCollection : ICollider
    {
        public ICollider[] Colliders { get; }
        public RectangleF BoundingBox { get; }

        public ColliderCollection(params ICollider[] colliders)
        {
            Colliders = colliders;
            BoundingBox = GetBoundingBox();
        }

        private RectangleF GetBoundingBox()
        {
            var (l, t, r, b) = (float.MaxValue, float.MaxValue, float.MinValue, float.MinValue);

            foreach (var collider in Colliders)
            {
                l = Math.Min(l, collider.BoundingBox.Left);
                t = Math.Min(t, collider.BoundingBox.Top);
                r = Math.Max(r, collider.BoundingBox.Right);
                b = Math.Max(b, collider.BoundingBox.Bottom);
            }

            return new RectangleF(l, t, r - l, b - t);
        }

        public bool IntersectsWith(ICollider other)
        {
            return Colliders.Any(collider => collider.IntersectsWith(other));
        }

        public ICollider SetLocation(Vector2 pos) => throw new NotImplementedException();
    }
}
