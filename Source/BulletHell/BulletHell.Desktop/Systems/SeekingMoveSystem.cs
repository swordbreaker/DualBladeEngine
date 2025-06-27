using BulletHell.Desktop.Components;
using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using Microsoft.Xna.Framework;
using System;

namespace BulletHell.Desktop.Systems;

public class SeekingMoveSystem(IGameContext gameContext) : ComponentSystem<SeekingMoveComponent, TransformComponent>(gameContext)
{
    protected override void Update(ref SeekingMoveComponent seeking, ref TransformComponent transform, ref IEntity entity, GameTime gameTime)
    {
        if (!seeking.HasTarget)
            return;
            
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        var currentTime = (float)gameTime.TotalGameTime.TotalSeconds;
        
        // Update target velocity prediction
        if (currentTime - seeking.LastTargetUpdateTime > 0.1f) // Update every 0.1 seconds
        {
            var timeDelta = currentTime - seeking.LastTargetUpdateTime;
            if (timeDelta > 0)
            {
                seeking.PredictedTargetVelocity = (seeking.TargetPosition - seeking.LastTargetPosition) / timeDelta;
            }
            seeking.LastTargetPosition = seeking.TargetPosition;
            seeking.LastTargetUpdateTime = currentTime;
        }
        
        // Predict where the target will be
        var predictedPosition = seeking.TargetPosition + seeking.PredictedTargetVelocity * seeking.PredictionTime;
        
        // Calculate desired velocity (steering towards predicted position)
        var desiredDirection = predictedPosition - transform.Position;
        var distance = desiredDirection.Length();
        
        if (distance > 0)
        {
            desiredDirection.Normalize();
            
            // Slow down when approaching the target
            var desiredSpeed = seeking.MaxSpeed;
            if (distance < seeking.ArrivalRadius)
            {
                desiredSpeed *= (distance / seeking.ArrivalRadius);
            }
            
            var desiredVelocity = desiredDirection * desiredSpeed;
            
            // Calculate steering force
            var steer = (desiredVelocity - seeking.Velocity) * seeking.SeekingForce;
            
            // Apply steering
            seeking.Velocity += steer * deltaTime;
            
            // Limit velocity to max speed
            if (seeking.Velocity.Length() > seeking.MaxSpeed)
            {
                seeking.Velocity = Vector2.Normalize(seeking.Velocity) * seeking.MaxSpeed;
            }
        }
        
        // Update position
        transform.Position += seeking.Velocity * deltaTime;
    }
}
