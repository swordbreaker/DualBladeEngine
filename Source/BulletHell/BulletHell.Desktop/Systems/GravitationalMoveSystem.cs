using BulletHell.Desktop.Components;
using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;

namespace BulletHell.Desktop.Systems;

public class GravitationalMoveSystem(IGameContext gameContext) : ComponentSystem<GravitationalMoveComponent, TransformComponent>(gameContext)
{
    protected override void Update(ref GravitationalMoveComponent gravity, ref TransformComponent transform, ref IEntity entity, GameTime gameTime)
    {
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Calculate distance and direction to gravity center
        var directionToCenter = gravity.GravityCenter - transform.Position;
        var distance = directionToCenter.Length();

        // Prevent infinite acceleration when too close
        distance = Math.Max(distance, gravity.MinDistance);

        if (distance > 0)
        {
            var normalizedDirection = directionToCenter / distance;

            // Calculate gravitational force (F = G * m1 * m2 / r^2, simplified)
            var force = gravity.GravityStrength * gravity.Mass / (distance * distance);
            force = Math.Min(force, gravity.MaxForce); // Cap the maximum force

            // Apply force (attraction or repulsion)
            var acceleration = normalizedDirection * force * (gravity.IsAttraction ? 1 : -1);
            gravity.Velocity += acceleration * deltaTime;
        }

        // Update position
        transform.Position += gravity.Velocity * deltaTime;
    }
}
