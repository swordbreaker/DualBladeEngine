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
    protected override void Initialize(KinematicComponent component)
    {
        base.Initialize(component);
        if (component.Entity.GetComponent<TransformComponent>() is null)
        {
            throw new Exception("KinematicComponent must have a TransformComponent");
        }
    }

    protected override void Update(KinematicComponent component, GameTime gameTime)
    {
        var entity = component.Entity;
        var transform = entity.GetComponent<TransformComponent>();

        transform.Position = component.PhysicsBody.Position;
        transform.Rotation = component.PhysicsBody.Rotation;
    }
}
