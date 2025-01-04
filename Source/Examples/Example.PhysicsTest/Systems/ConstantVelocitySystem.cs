using DualBlade._2D.BladePhysics.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using Example.PhysicsTest.Components;

namespace Example.PhysicsTest.Systems;

public class ConstantVelocitySystem(IGameContext context) : ComponentSystem<RigidBody, ConstantVelocityComponent>(context)
{
    protected override void FixedUpdate(ref RigidBody body, ref ConstantVelocityComponent constantVelocity, ref IEntity entity, GameTime gameTime)
    {
        body.Velocity = constantVelocity.Velocity;
    }
}
