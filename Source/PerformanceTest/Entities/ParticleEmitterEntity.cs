using DualBlade._2D.Rendering.Entities;
using PerformanceTest.Components;

namespace PerformanceTest.Entities;
public class ParticleEmitterEntity : TransformEntity
{
    public ParticleEmitterEntity() : base()
    {
        AddComponent<ParticleEmitterComponent>();
    }
}
