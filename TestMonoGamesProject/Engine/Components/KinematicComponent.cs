using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;
using TestMonoGamesProject.Engine.Entities;

namespace TestMonoGamesProject.Engine.Components
{
    public class KinematicComponent : IComponent
    {
        public Body PhysicsBody = new();

        public Vector2 VerticalVelocity = Vector2.Zero;
        public IEntity Entity { get; init; }
    }
}
