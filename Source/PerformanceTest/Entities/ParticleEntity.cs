using DualBlade._2D.Rendering.Entities;
using DualBlade.Core.Rendering;
using DualBlade.Core.Services;
using PerformanceTest.Components;

namespace PerformanceTest.Entities;
public class ParticleEntity : SpriteEntity
{
    public ParticleComponent ParticleComponent { get; }

    public ParticleEntity(IGameEngine gameEngine, ISpriteFactory spriteFactory) : base()
    {
        ParticleComponent = AddComponent<ParticleComponent>();
        this.Renderer.SetSprite(spriteFactory.CreateWhitePixelSprite());
    }
}
