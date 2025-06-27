using BulletHell.Desktop.Components;
using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;

namespace BulletHell.Desktop.Systems;

public class RocketMoveSystem(IGameContext gameContext) : ComponentSystem<TransformComponent, RocketMoveComponent>(gameContext)
{
    protected override void Update(ref TransformComponent transform, ref RocketMoveComponent rocket, ref IEntity entity, GameTime gameTime)
    {
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        var playerEntity = Ecs.GetEntity(rocket.TargetTransform);
        var targetPos = playerEntity.Value.Component<TransformComponent>().Position;

        // Apply gravity (downward force)
        var gravity = new Vector2(0, -9.81f * 0.01f); // Scaled gravity for game units
        rocket.Velocity += gravity * deltaTime;

        // Calculate direction to target
        var directionToTarget = targetPos - transform.Position;

        // If we're close enough to the target, don't apply steering
        if (targetPos.Y < transform.Position.Y)
        {
            // Normalize direction and apply steering force
            var normalizedDirection = Vector2.Normalize(directionToTarget);
            var steeringForce = normalizedDirection * rocket.Speed * 2 * deltaTime;

            // Apply steering with some smoothing to make movement more realistic
            rocket.Velocity += steeringForce * 0.5f;
        }

        // Apply velocity to position
        transform.Position += rocket.Velocity * deltaTime;

        // Update rotation to point in direction of movement
        if (rocket.Velocity.Length() > 0.1f) // Only update rotation if moving
        {
            var angle = (float)Math.Atan2(rocket.Velocity.Y, -rocket.Velocity.X);
            transform.Rotation = MathHelper.ToDegrees(angle) - 90f; // Subtract 90 degrees since sprite looks up
        }

        // Optional: Limit maximum velocity to prevent unrealistic speeds
        var maxVelocity = 100f;
        if (rocket.Velocity.Length() > maxVelocity)
        {
            rocket.Velocity = Vector2.Normalize(rocket.Velocity) * maxVelocity;
        }
    }
}
