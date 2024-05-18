using Microsoft.Xna.Framework;
using MonoGameEngine.Engine.Entities;
using nkast.Aether.Physics2D.Dynamics;

namespace MonoGameEngine.Engine.Components;

public class KinematicComponent : IComponent
{
    public Body PhysicsBody = new();

    public Vector2 VerticalVelocity = Vector2.Zero;
    public IEntity Entity { get; init; }
}
