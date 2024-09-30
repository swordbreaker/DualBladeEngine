using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Extensions;
using DualBlade.Core.Rendering;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using Microsoft.Xna.Framework;
using PerformanceTest.Components;
using PerformanceTest.Entities;
using System;

namespace PerformanceTest.Systems;
public class ParticleEmitterSystem(IGameContext gameContext) : ComponentSystem<ParticleEmitterComponent, TransformComponent>(gameContext)
{
    private readonly ISpriteFactory spriteFactory = gameContext.GameEngine.SpriteFactory;
    private readonly Random random = new();

    protected override void Update(ref ParticleEmitterComponent particle, ref TransformComponent transform, ref IEntity entity, GameTime gameTime)
    {
        if (gameTime.TotalGameTime.TotalSeconds - particle.LastEmitTime > particle.EmitRated)
        {
            particle.LastEmitTime = (float)gameTime.TotalGameTime.TotalSeconds;
            EmitParticle(transform.Position);
        }
    }

    private void EmitParticle(Vector2 position)
    {
        var particle = new ParticleEntity(
            GameContext.GameEngine,
            spriteFactory,
            position,
             new Vector2(random.NextFloat(1f, 10f)),
             Color.Lerp(Color.Red, Color.Yellow, random.NextSingle()),
             new Vector2(0, -9.81f),
             new Vector2(random.Next(-5, 5), random.NextFloat(1, 5)));

        World.AddEntity(particle);
    }
}
