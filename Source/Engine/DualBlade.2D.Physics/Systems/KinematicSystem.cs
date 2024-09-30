using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using DualBlade._2D.Physics.Components;
using DualBlade.Core.Entities;

namespace DualBlade._2D.Physics.Systems;

public class KinematicSystem(IGameContext gameContext) : ComponentSystem<KinematicComponent, TransformComponent>(gameContext)
{
    protected override void Update(ref KinematicComponent kinematic, ref TransformComponent transform, ref IEntity entity, GameTime gameTime)
    {
        transform.Position = kinematic.PhysicsBody.Position;
        transform.Rotation = kinematic.PhysicsBody.Rotation;
    }
}
