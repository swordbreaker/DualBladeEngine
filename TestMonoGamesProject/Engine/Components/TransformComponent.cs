using FunctionalMonads.Monads.MaybeMonad;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using TestMonoGamesProject.Engine.Entities;

namespace TestMonoGamesProject.Engine.Components
{
    public class TransformComponent : IComponent
    {
        public Vector2 Position = Vector2.Zero;
        public float Rotation = 0f;
        public Vector2 Scale = Vector2.One;
        public IMaybe<TransformComponent> Parent = Maybe.None<TransformComponent>();
        public List<TransformComponent> Children = [];

        public IEntity Entity { get; init; }
    }
}
