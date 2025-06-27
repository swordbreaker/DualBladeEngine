using DualBlade.Core.Components;

namespace BulletHell.Desktop.Components;

public partial struct OrbitMoveComponent : IComponent
{
    public Vector2 CenterPoint;
    public float OrbitRadius;
    public float OrbitSpeed; // Radians per second
    public float CurrentAngle;
    public float MoveSpeed; // Speed at which the center point moves
    public Vector2 Direction; // Direction the center point moves
}
