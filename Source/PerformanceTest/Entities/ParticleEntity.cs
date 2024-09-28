using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Rendering;
using DualBlade.Core.Services;
using Microsoft.Xna.Framework;
using PerformanceTest.Components;

namespace PerformanceTest.Entities;
public partial struct ParticleEntity : IEntity
{
    public ParticleEntity(
        IGameEngine gameEngine,
        ISpriteFactory spriteFactory,
        Vector2 pos,
        Vector2 scale,
        Color color,
        Vector2 acceleration,
        Vector2 velocity)
    {
        var renderComponent = new RenderComponent()
        {
            Color = color,
        };
        renderComponent.SetSprite(spriteFactory.CreateWhitePixelSprite());
        var particleComponent = new ParticleComponent()
        {
            Acceleration = acceleration,
            Velocity = velocity,
        };

        AddComponent(new TransformComponent
        {
            Position = pos,
            Scale = scale,
            Rotation = 0,
        });
        AddComponent(renderComponent);
        AddComponent(particleComponent);
    }
}
