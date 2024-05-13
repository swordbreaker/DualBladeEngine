using Microsoft.Xna.Framework;
using System;
using System.Drawing;

namespace TestMonoGamesProject.Engine.Physics
{
    public class RectCollider(RectangleF Rectangle) : ICollider
    {
        public Guid Id { get; init; } = Guid.NewGuid();

        public RectangleF BoundingBox => Rectangle;

        public bool IntersectsWith(ICollider other) =>
            Rectangle.IntersectsWith(other.BoundingBox);

        public ICollider SetLocation(Vector2 pos) =>
            new RectCollider(new RectangleF(new PointF(pos.X, pos.Y), Rectangle.Size)) { Id = this.Id };

        public override bool Equals(object? obj) =>
            obj is RectCollider collider && Id.Equals(collider.Id);

        public override int GetHashCode() =>
            HashCode.Combine(Id);
    }
}