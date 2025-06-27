using DualBlade.Core.Components;

namespace BulletHell.Desktop.Components;

public partial struct AccelerationMoveComponent : IComponent
{
    public Vector2 Velocity;
    public Vector2 Acceleration;
    public float MaxSpeed;
    public float Drag; // Optional drag to limit acceleration buildup
}
