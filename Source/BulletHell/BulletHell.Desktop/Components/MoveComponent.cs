using DualBlade.Core.Components;


namespace BulletHell.Desktop.Components;

public partial struct MoveComponent : IComponent
{
    public Vector2 Velocity { get; set; }
}
