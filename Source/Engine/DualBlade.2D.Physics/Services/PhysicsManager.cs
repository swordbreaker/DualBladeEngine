using nkast.Aether.Physics2D.Dynamics;
using System.Diagnostics.CodeAnalysis;

namespace DualBlade._2D.Physics.Services;

[ExcludeFromCodeCoverage(Justification = "External library")]
public class PhysicsManager : World, IPhysicsManager
{
    object IPhysicsManager.Tag { get => Tag; set => Tag = value; }
    BodyDelegate IPhysicsManager.BodyAdded { get => BodyAdded; set => BodyAdded = value; }
    BodyDelegate IPhysicsManager.BodyRemoved { get => BodyRemoved; set => BodyRemoved = value; }
    FixtureDelegate IPhysicsManager.FixtureAdded { get => FixtureAdded; set => FixtureAdded = value; }
    FixtureDelegate IPhysicsManager.FixtureRemoved { get => FixtureRemoved; set => FixtureRemoved = value; }
    JointDelegate IPhysicsManager.JointAdded { get => JointAdded; set => JointAdded = value; }
    JointDelegate IPhysicsManager.JointRemoved { get => JointRemoved; set => JointRemoved = value; }
    ControllerDelegate IPhysicsManager.ControllerAdded { get => ControllerAdded; set => ControllerAdded = value; }
    ControllerDelegate IPhysicsManager.ControllerRemoved { get => ControllerRemoved; set => ControllerRemoved = value; }

    ContactManager IPhysicsManager.ContactManager => ContactManager;

    BodyCollection IPhysicsManager.BodyList => BodyList;

    JointCollection IPhysicsManager.JointList => JointList;

    public PhysicsManager(Vector2 gravity) : base(gravity)
    {
    }

    public PhysicsManager() : base(new Vector2(0f, -20f))
    {
    }
}