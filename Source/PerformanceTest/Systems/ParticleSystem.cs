using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using Microsoft.Xna.Framework;
using PerformanceTest.Components;

namespace PerformanceTest.Systems;
internal class ParticleSystem(IGameContext gameContext) : ComponentSystem<ParticleComponent>(gameContext)
{
    protected override void Update(ref ParticleComponent component, GameTime gameTime)
    {
        if (component.TimeToLive <= 0)
        {
            Ecs.DestroyEntity(component);
            return;
        }

        using var transformProxy = Ecs.GetAdjacentComponent<TransformComponent>(component).Value.GetProxy();
        transformProxy.Value.Position += component.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        component.Velocity += component.Acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
        component.TimeToLive -= (float)gameTime.ElapsedGameTime.TotalSeconds;
    }
}
