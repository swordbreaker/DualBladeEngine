using System.Linq;
using BulletHell.Desktop.Components;
using BulletHell.Desktop.Entities;
using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;

namespace BulletHell.Desktop.Systems;

public class HomingMoveSystem(IGameContext context) : ComponentSystem<TransformComponent, HomingMoveComponent>(context)
{
    protected override void Update(ref TransformComponent transform, ref HomingMoveComponent homing, ref IEntity entity, GameTime gameTime)
    {
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Try to find player if no target
        if (!homing.HasTarget)
        {
            TryFindPlayerTarget(transform.Position, ref homing);
        }

        // Update movement
        if (homing.HasTarget)
        {
            // Check if target is still valid and within reasonable distance
            var distanceToTarget = Vector2.Distance(transform.Position, homing.TargetPosition);
            
            // If target is too far away (bullet has missed or overshot), lose target
            if (distanceToTarget > homing.AcquisitionRange * 2.0f)
            {
                homing.HasTarget = false;
            }
            else
            {
                // Update target position if target entity still exists
                TryUpdateTargetPosition(ref homing);
                
                // Calculate direction to target
                var directionToTarget = Vector2.Normalize(homing.TargetPosition - transform.Position);

                // Calculate desired velocity
                var desiredVelocity = directionToTarget * homing.MaxSpeed;

                // Calculate steering force with reduced intensity as distance increases
                var steeringIntensity = MathHelper.Clamp(1.0f - (distanceToTarget / homing.AcquisitionRange), 0.2f, 1.0f);
                var steering = (desiredVelocity - homing.Velocity) * homing.TurnSpeed * steeringIntensity * deltaTime;

                // Apply steering
                homing.Velocity += steering;

                // Limit to max speed
                if (homing.Velocity.Length() > homing.MaxSpeed)
                {
                    homing.Velocity = Vector2.Normalize(homing.Velocity) * homing.MaxSpeed;
                }
            }
        }

        // Apply velocity
        transform.Position += homing.Velocity * deltaTime;

        // Update rotation to face movement direction
        if (homing.Velocity.Length() > 0.1f)
        {
            transform.Rotation = MathHelper.ToDegrees(MathF.Atan2(homing.Velocity.Y, homing.Velocity.X));
        }
    }

    private void TryFindPlayerTarget(Vector2 position, ref HomingMoveComponent homing)
    {
        // Find player entity within acquisition range
        foreach (var player in World.Entities.Where(e => e.HasComponent<PlayerComponent>()))
        {
            var playerTransform = player.Component<TransformComponent>();
            var distance = Vector2.Distance(position, playerTransform.Position);

            if (distance <= homing.AcquisitionRange)
            {
                homing.HasTarget = true;
                homing.TargetPosition = playerTransform.Position;
                homing.TargetEntityId = player.Id;
                break;
            }
        }
    }

    private void TryUpdateTargetPosition(ref HomingMoveComponent homing)
    {
        // Try to update target position if target entity still exists
        var targetEntityId = homing.TargetEntityId;
        var targetEntity = World.Entities.FirstOrDefault(e => e.Id == targetEntityId);
        if (targetEntity?.HasComponent<TransformComponent>() == true)
        {
            var targetTransform = targetEntity.Component<TransformComponent>();
            homing.TargetPosition = targetTransform.Position;
        }
        else
        {
            // Target entity no longer exists, lose target
            homing.HasTarget = false;
        }
    }
}
