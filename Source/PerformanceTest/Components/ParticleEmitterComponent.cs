using DualBlade.Core.Components;

namespace PerformanceTest.Components;

public class ParticleEmitterComponent : ComponentBase
{
    public float EmitRated = 0.01f;
    public float LastEmitTime;
}
