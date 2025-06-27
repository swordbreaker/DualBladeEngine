using DualBlade.Core.Components;
using Microsoft.Xna.Framework;

namespace BulletHell.Desktop.Components;

public partial struct ZigzagMoveComponent : IComponent
{
    public Vector2 BaseDirection { get; set; }
    public float BaseSpeed { get; set; }
    public float ZigzagAmplitude { get; set; }
    public float ZigzagFrequency { get; set; }
    public float CurrentTime { get; set; }
    public Vector2 PerpendicularDirection { get; set; }
}
