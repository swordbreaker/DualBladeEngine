using FunctionalMonads.Monads.MaybeMonad;
using MonoGameEngine.Engine.Entities;
using System.Collections.Generic;

namespace MonoGameEngine.Engine.Components;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class TransformComponent : IComponent
{
    public Vector2 Position = Vector2.Zero;
    public float Rotation = 0f;
    public Vector2 Scale = Vector2.One;
    public IMaybe<TransformComponent> Parent = Maybe.None<TransformComponent>();
    public List<TransformComponent> Children = [];

    public IEntity Entity { get; init; }
}
