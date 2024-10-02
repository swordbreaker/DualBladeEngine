using DualBlade.Core.Components;
using nkast.Aether.Physics2D.Dynamics;

namespace DualBlade._2D.Physics.Components;

public partial struct KinematicComponent : IComponent
{
    public Body PhysicsBody = new();
    public Vector2 VerticalVelocity = Vector2.Zero;
}
