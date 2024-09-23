using DualBlade.Core.Components;
using DualBlade.Core.Entities;
using nkast.Aether.Physics2D.Dynamics;

namespace DualBlade._2D.Physics.Components;

public class KinematicComponent : IComponent
{
    public Body PhysicsBody = new();

    public Vector2 VerticalVelocity = Vector2.Zero;
    public IEntity Entity { get; init; }
}
