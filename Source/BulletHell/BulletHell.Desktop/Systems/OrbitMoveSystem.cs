using BulletHell.Desktop.Components;
using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;

namespace BulletHell.Desktop.Systems;

public class OrbitMoveSystem(IGameContext context) : ComponentSystem<TransformComponent, OrbitMoveComponent>(context)
{
    protected override void Update(ref TransformComponent transform, ref OrbitMoveComponent orbit, ref IEntity entity, GameTime gameTime)
    {
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Update orbit angle
        orbit.CurrentAngle += orbit.OrbitSpeed * deltaTime;

        // Keep angle in range [0, 2π]
        orbit.CurrentAngle = orbit.CurrentAngle % MathHelper.TwoPi;

        // Move the center point
        orbit.CenterPoint += orbit.Direction * orbit.MoveSpeed * deltaTime;

        // Calculate position on orbit
        var orbitOffset = new Vector2(
            MathF.Cos(orbit.CurrentAngle) * orbit.OrbitRadius,
            MathF.Sin(orbit.CurrentAngle) * orbit.OrbitRadius
        );

        // Set final position
        transform.Position = orbit.CenterPoint + orbitOffset;

        // Update rotation to face orbit direction
        var orbitVelocity = new Vector2(
            -MathF.Sin(orbit.CurrentAngle) * orbit.OrbitRadius * orbit.OrbitSpeed,
            MathF.Cos(orbit.CurrentAngle) * orbit.OrbitRadius * orbit.OrbitSpeed
        ) + orbit.Direction * orbit.MoveSpeed;

        if (orbitVelocity.Length() > 0.1f)
        {
            transform.Rotation = MathHelper.ToDegrees(MathF.Atan2(orbitVelocity.Y, orbitVelocity.X));
        }
    }
}
