using BulletHell.Desktop.Components;
using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;

namespace BulletHell.Desktop.Systems;

public class SpiralMoveSystem(IGameContext context) : ComponentSystem<TransformComponent, SpiralMoveComponent>(context)
{
    protected override void Update(ref TransformComponent transform, ref SpiralMoveComponent spiral, ref IEntity entity, GameTime gameTime)
    {
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Update angle
        spiral.CurrentAngle += spiral.AngularVelocity * deltaTime;

        // Calculate spiral position
        var radius = spiral.RadiusGrowth * spiral.CurrentAngle;
        var offset = new Vector2(
            MathF.Cos(spiral.CurrentAngle) * radius,
            MathF.Sin(spiral.CurrentAngle) * radius
        );

        // Move center point
        spiral.CenterPoint += spiral.InitialVelocity * deltaTime;

        // Update position
        transform.Position = spiral.CenterPoint + offset;

        // Update rotation to face movement direction
        if (spiral.AngularVelocity != 0)
        {
            var tangentAngle = spiral.CurrentAngle + MathHelper.PiOver2;
            transform.Rotation = MathHelper.ToDegrees(tangentAngle);
        }
    }
}
