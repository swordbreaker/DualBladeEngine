using BulletHell.Desktop.Components;
using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;

namespace BulletHell.Desktop.Systems;

public class WaveMoveSystem(IGameContext context) : ComponentSystem<TransformComponent, WaveMoveComponent>(context)
{
    protected override void Update(ref TransformComponent transform, ref WaveMoveComponent wave, ref IEntity entity, GameTime gameTime)
    {
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Update phase
        wave.Phase += wave.Frequency * deltaTime;

        // Calculate wave offset
        var waveOffset = MathF.Sin(wave.Phase) * wave.Amplitude;
        var perpendicular = wave.PerpendicularDirection * waveOffset;

        // Move in main direction plus wave offset
        var movement = (wave.Direction * wave.Speed * deltaTime) + (perpendicular * deltaTime * wave.Frequency);
        transform.Position += movement;

        // Update rotation to face movement direction
        var actualDirection = Vector2.Normalize(wave.Direction + perpendicular * MathF.Cos(wave.Phase));
        transform.Rotation = MathHelper.ToDegrees(MathF.Atan2(actualDirection.Y, actualDirection.X));
    }
}
