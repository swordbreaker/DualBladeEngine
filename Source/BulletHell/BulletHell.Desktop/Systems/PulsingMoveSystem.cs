using BulletHell.Desktop.Components;
using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using Microsoft.Xna.Framework;
using System;

namespace BulletHell.Desktop.Systems;

public class PulsingMoveSystem(IGameContext gameContext) : ComponentSystem<PulsingMoveComponent, TransformComponent>(gameContext)
{
    protected override void Update(ref PulsingMoveComponent pulsing, ref TransformComponent transform, ref IEntity entity, GameTime gameTime)
    {
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        pulsing.CurrentTime += deltaTime;

        // Calculate pulsing speed
        var speedMultiplier = 1.0f + (float)Math.Sin(pulsing.CurrentTime * pulsing.PulseFrequency) * pulsing.PulseAmplitude;
        var currentSpeed = pulsing.BaseSpeed * speedMultiplier;

        // Move the bullet
        transform.Position += pulsing.Direction * currentSpeed * deltaTime;

        // Update scale for visual pulsing effect
        var scaleMultiplier = pulsing.BaseScale + (float)Math.Sin(pulsing.CurrentTime * pulsing.PulseFrequency) * pulsing.ScalePulseAmplitude;
        transform.Scale = Vector2.One * Math.Max(0.1f, scaleMultiplier);
    }
}
