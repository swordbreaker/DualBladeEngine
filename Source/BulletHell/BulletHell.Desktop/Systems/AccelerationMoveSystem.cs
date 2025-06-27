using BulletHell.Desktop.Components;
using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;

namespace BulletHell.Desktop.Systems;

public class AccelerationMoveSystem(IGameContext context) : ComponentSystem<TransformComponent, AccelerationMoveComponent>(context)
{
    protected override void Update(ref TransformComponent transform, ref AccelerationMoveComponent accel, ref IEntity entity, GameTime gameTime)
    {
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Apply acceleration
        accel.Velocity += accel.Acceleration * deltaTime;

        // Apply drag if specified
        if (accel.Drag > 0)
        {
            accel.Velocity *= MathF.Pow(1 - accel.Drag, deltaTime);
        }

        // Limit to max speed
        if (accel.Velocity.Length() > accel.MaxSpeed)
        {
            accel.Velocity = Vector2.Normalize(accel.Velocity) * accel.MaxSpeed;
        }

        // Apply velocity to position
        transform.Position += accel.Velocity * deltaTime;

        // Update rotation to face movement direction
        if (accel.Velocity.Length() > 0.1f)
        {
            transform.Rotation = MathHelper.ToDegrees(MathF.Atan2(accel.Velocity.Y, accel.Velocity.X));
        }
    }
}
