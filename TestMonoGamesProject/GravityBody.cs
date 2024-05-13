using Microsoft.Xna.Framework;
using System.Drawing;
using TestMonoGamesProject.Engine.Physics;

namespace TestMonoGamesProject
{
    public class GravityBody
    {
        public static float G = 9.81f * 10;

        private readonly Vector2 V0;
        private Vector2 _v;
        private readonly float _mass;
        private readonly Vector2 _a = new(0f, G);
        private readonly PhysicsManager _physicsManager;
        private PointF _size;

        public Vector2 Pos;

        public GravityBody(Vector2 v0, Vector2 pos0, float mass, PointF size, PhysicsManager physicsManager)
        {
            V0 = v0;
            _v = V0;
            _mass = mass;
            Pos = pos0;
            _size = size;
            _physicsManager = physicsManager;
        }

        private RectangleF GetBounds(Vector2 pos) =>
            new(pos.X - _size.X / 2, pos.Y - _size.Y / 2, _size.X, _size.Y);

        public void AddForce(Vector2 force) => _v += force / _mass;

        public Vector2 Update(float dt)
        {
            var newPos = Pos + _v * dt * _mass;
            var collider = new RectCollider(GetBounds(newPos));

            if (_physicsManager.CheckCollision(collider))
            {
                return Pos;
            }

            Pos += _v * dt * _mass;
            _v += _a * dt * _mass;

            return Pos;
        }
    }
}
