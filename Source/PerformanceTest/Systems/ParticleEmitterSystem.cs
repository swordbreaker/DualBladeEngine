using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Extensions;
using DualBlade.Core.Rendering;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using Microsoft.Xna.Framework;
using PerformanceTest.Components;
using PerformanceTest.Entities;
using System;

namespace PerformanceTest.Systems;
public class ParticleEmitterSystem(IGameContext gameContext) : ComponentSystem<ParticleEmitterComponent>(gameContext)
{
    private readonly ISpriteFactory spriteFactory = gameContext.GameEngine.SpriteFactory;
    private readonly Random random = new();

    protected override void Update(ParticleEmitterComponent component, GameTime gameTime)
    {
        base.Update(component, gameTime);

        if (gameTime.TotalGameTime.TotalSeconds - component.LastEmitTime > component.EmitRated)
        {
            component.LastEmitTime = (float)gameTime.TotalGameTime.TotalSeconds;
            EmitParticle(component.Entity.GetComponent<TransformComponent>().Position);
        }
    }

    private void EmitParticle(Vector2 position)
    {
        var particle = new ParticleEntity(GameContext.GameEngine, spriteFactory);
        particle.Renderer.Color = Color.Lerp(Color.Red, Color.Yellow, random.NextSingle());
        particle.Transform.Position = position;
        particle.Transform.Scale = new Vector2(random.NextFloat(1f, 10f));
        particle.ParticleComponent.Acceleration = new Vector2(0, -9.81f);
        particle.ParticleComponent.Velocity = new Vector2(random.Next(-5, 5), random.NextFloat(1, 5));

        World.AddEntity(particle);
    }
}
