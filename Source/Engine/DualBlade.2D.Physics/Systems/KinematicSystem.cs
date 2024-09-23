using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using System;
using DualBlade.Core.Extensions;
using FunctionalMonads.Monads.MaybeMonad;
using DualBlade._2D.Physics.Components;

namespace DualBlade._2D.Physics.Systems;

public class KinematicSystem(IGameContext gameContext) : ComponentSystem<KinematicComponent>(gameContext)
{
    protected override void Update(KinematicComponent component, GameTime gameTime)
    {
        var entity = component.Entity;
        var transform = entity.GetComponent<TransformComponent>()
            .SomeOrProvided(() =>
                throw new Exception($"A Entity with a {nameof(KinematicComponent)} must have a {nameof(TransformComponent)}"));

        transform.Position = component.PhysicsBody.Position;
        transform.Rotation = component.PhysicsBody.Rotation;
    }
}
