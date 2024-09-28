using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using System;
using DualBlade._2D.Physics.Components;

namespace DualBlade._2D.Physics.Systems;

public class KinematicSystem(IGameContext gameContext) : ComponentSystem<KinematicComponent>(gameContext)
{
    protected override void OnAdded(ref KinematicComponent component)
    {
        if (!Ecs.GetAdjacentComponent<TransformComponent>(component).HasValue)
        {
            throw new Exception("KinematicComponent must have a TransformComponent");
        }
    }

    protected override void Update(ref KinematicComponent component, GameTime gameTime)
    {
        using var transformProxy = Ecs.GetAdjacentComponent<TransformComponent>(component).Value.GetProxy();

        transformProxy.Value.Position = component.PhysicsBody.Position;
        transformProxy.Value.Rotation = component.PhysicsBody.Rotation;
    }
}
