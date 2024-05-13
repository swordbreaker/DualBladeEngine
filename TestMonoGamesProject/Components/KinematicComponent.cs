using Microsoft.Xna.Framework;
using TestMonoGamesProject.Engine.Components;
using TestMonoGamesProject.Engine.Entities;

namespace TestMonoGamesProject.Components
{
    public class KinematicComponent : IComponent
    {
        public Vector2 Velocity = Vector2.Zero;
        public Vector2 KinematicVelocity = Vector2.Zero;
        public float Mass;
        public Vector2 Acceleration;

        public IEntity Entity { get; init; }
    }
}
