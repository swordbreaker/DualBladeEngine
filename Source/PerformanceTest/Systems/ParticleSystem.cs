using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Extensions;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using Microsoft.Xna.Framework;
using PerformanceTest.Components;

namespace PerformanceTest.Systems;
internal class ParticleSystem(IGameContext gameContext) : ComponentSystem<ParticleComponent>(gameContext)
{
    protected override void Update(ParticleComponent component, GameTime gameTime)
    {
        base.Update(component, gameTime);

        if (component.TimeToLive <= 0)
        {
            World.Destroy(component.Entity);
            return;
        }

        component.Entity.GetComponent<TransformComponent>().Position += component.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        component.Velocity += component.Acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
        component.TimeToLive -= (float)gameTime.ElapsedGameTime.TotalSeconds;
    }
}
