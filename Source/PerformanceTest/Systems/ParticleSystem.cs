using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using Microsoft.Xna.Framework;
using PerformanceTest.Components;

namespace PerformanceTest.Systems;
internal class ParticleSystem(IGameContext gameContext) : ComponentSystem<ParticleComponent, TransformComponent>(gameContext)
{
    protected override void Update(ref ParticleComponent particle, ref TransformComponent transform, ref IEntity entity, GameTime gameTime)
    {
        if (particle.TimeToLive <= 0)
        {
            Ecs.DestroyEntity(entity);
            return;
        }

        transform.Position += particle.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        particle.Velocity += particle.Acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
        particle.TimeToLive -= (float)gameTime.ElapsedGameTime.TotalSeconds;
    }
}
