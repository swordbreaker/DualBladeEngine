using DualBlade._2D.Rendering.Components;

namespace BulletHell.Desktop.Components;

public partial struct RocketMoveComponent : IComponent
{
    public Vector2 Velocity;
    public TransformComponent TargetTransform;
    public float Speed;
}
