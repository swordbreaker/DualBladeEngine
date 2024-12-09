using DualBlade.Core.Components;

namespace DualBlade._2D.BladePhysics.Components;
public partial struct RigidBody : IComponent
{
    public Vector2 Velocity;
    public Vector2 Acceleration;
    public float Mass = 1;
    public bool CollectCollisionEvents;
}
