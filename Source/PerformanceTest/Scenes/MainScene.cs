using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Scenes;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using Microsoft.Xna.Framework;
using PerformanceTest.Entities;
using PerformanceTest.Systems;
using System.Collections.Generic;

namespace PerformanceTest.Scenes;
public class MainScene(IGameContext context) : GameScene(context)
{
    public override IEnumerable<ISystem> SetupSystems()
    {
        yield return CreateSystem<ParticleSystem>();
        yield return CreateSystem<ParticleEmitterSystem>();
    }

    protected override IEnumerable<INodeEntity> SetupEntities()
    {
        for (float x = -5; x < 5; x += 2f)
        {
            for (float y = -5; y < 5; y += 2f)
            {
                var entity = new ParticleEmitterEntity();
                entity.Transform.Position = new Vector2(x, y);
                yield return entity;
            }
        }
    }
}
