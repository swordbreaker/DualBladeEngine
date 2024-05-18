using MonoGameEngine.Engine.Entities;
using nkast.Aether.Physics2D.Dynamics;

namespace MonoGameEngine.Engine.Components;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class KinematicComponent : IComponent
{
    public Body PhysicsBody = new();

    public Vector2 VerticalVelocity = Vector2.Zero;
    public IEntity Entity { get; init; }
}
