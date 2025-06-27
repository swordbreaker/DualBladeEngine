using DualBlade.Core.Components;
using Microsoft.Xna.Framework;

namespace BulletHell.Desktop.Components;

public partial struct PulsingMoveComponent : IComponent
{
    public Vector2 Direction { get; set; }
    public float BaseSpeed { get; set; }
    public float PulseAmplitude { get; set; }
    public float PulseFrequency { get; set; }
    public float CurrentTime { get; set; }
    public float BaseScale { get; set; }
    public float ScalePulseAmplitude { get; set; }
}
