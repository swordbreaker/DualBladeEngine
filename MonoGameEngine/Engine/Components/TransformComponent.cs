using FunctionalMonads.Monads.MaybeMonad;
using Microsoft.Xna.Framework;
using MonoGameEngine.Engine.Entities;
using System.Collections.Generic;

namespace MonoGameEngine.Engine.Components;

public class TransformComponent : IComponent
{
    public Vector2 Position = Vector2.Zero;
    public float Rotation = 0f;
    public Vector2 Scale = Vector2.One;
    public IMaybe<TransformComponent> Parent = Maybe.None<TransformComponent>();
    public List<TransformComponent> Children = [];

    public IEntity Entity { get; init; }
}
