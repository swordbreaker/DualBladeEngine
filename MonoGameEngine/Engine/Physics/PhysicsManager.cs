using nkast.Aether.Physics2D.Dynamics;
using System.Diagnostics.CodeAnalysis;

namespace MonoGameEngine.Engine.Physics;

[ExcludeFromCodeCoverage(Justification = "External library")]
public class PhysicsManager : World, IPhysicsManager
{
    object IPhysicsManager.Tag { get => this.Tag; set => this.Tag = value; }
    BodyDelegate IPhysicsManager.BodyAdded { get => this.BodyAdded; set => this.BodyAdded = value; }
    BodyDelegate IPhysicsManager.BodyRemoved { get => this.BodyRemoved; set => this.BodyRemoved = value; }
    FixtureDelegate IPhysicsManager.FixtureAdded { get => this.FixtureAdded; set => this.FixtureAdded = value; }
    FixtureDelegate IPhysicsManager.FixtureRemoved { get => this.FixtureRemoved; set => this.FixtureRemoved = value; }
    JointDelegate IPhysicsManager.JointAdded { get => this.JointAdded; set => this.JointAdded = value; }
    JointDelegate IPhysicsManager.JointRemoved { get => this.JointRemoved; set => this.JointRemoved = value; }
    ControllerDelegate IPhysicsManager.ControllerAdded { get => this.ControllerAdded; set => this.ControllerAdded = value; }
    ControllerDelegate IPhysicsManager.ControllerRemoved { get => this.ControllerRemoved; set => this.ControllerRemoved = value; }

    ContactManager IPhysicsManager.ContactManager => this.ContactManager;

    BodyCollection IPhysicsManager.BodyList => this.BodyList;

    JointCollection IPhysicsManager.JointList => this.JointList;

    public PhysicsManager(Vector2 gravity) : base(gravity)
    {
    }

    public PhysicsManager() : base(new Vector2(0f, 9.80665f))
    {
    }
}