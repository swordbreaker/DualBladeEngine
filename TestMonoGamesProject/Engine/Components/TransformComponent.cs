using Microsoft.Xna.Framework;
using TestMonoGamesProject.Engine.Entities;

namespace TestMonoGamesProject.Engine.Components
{
    public class TransformComponent : IComponent
    {
        public Vector2 Position = Vector2.Zero;
        public float Rotation = 0f;
        public Vector2 Scale = Vector2.One;

        public IEntity Entity { get; init; }
    }
}
