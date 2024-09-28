using DualBlade.Core.Components;
using Microsoft.Xna.Framework;

namespace PerformanceTest.Components;
public partial struct ParticleComponent : IComponent
{
    public Vector2 Velocity;
    public Vector2 Acceleration;
    public float TimeToLive = 2f;
}
