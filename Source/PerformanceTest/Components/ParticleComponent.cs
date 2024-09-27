using DualBlade.Core.Components;
using Microsoft.Xna.Framework;

namespace PerformanceTest.Components;
public class ParticleComponent : ComponentBase
{
    public Vector2 Velocity;
    public Vector2 Acceleration;
    public float TimeToLive = 2f;
}
