using FunctionalMonads.Monads.MaybeMonad;
using MonoGameEngine.Engine.Components;
using MonoGameEngine.Engine.Extensions;
using MonoGameEngine.Engine.Worlds;
using System;

namespace MonoGameEngine.Engine.Systems;

public class KinematicSystem : ComponentSystem<KinematicComponent>
{
    protected override void Update(KinematicComponent component, GameTime gameTime, IGameEngine gameEngine)
    {
        var entity = component.Entity;
        var transform = entity.GetComponent<TransformComponent>()
            .SomeOrProvided(() =>
                throw new Exception($"A Entity with a {nameof(KinematicComponent)} must have a {nameof(TransformComponent)}"));

        transform.Position = component.PhysicsBody.Position;
        transform.Rotation = component.PhysicsBody.Rotation;
    }
}
