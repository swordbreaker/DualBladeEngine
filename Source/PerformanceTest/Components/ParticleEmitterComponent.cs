using DualBlade.Core.Components;

namespace PerformanceTest.Components;

public partial struct ParticleEmitterComponent : IComponent
{
    public float EmitRated = 0.01f;
    public float LastEmitTime;
}
