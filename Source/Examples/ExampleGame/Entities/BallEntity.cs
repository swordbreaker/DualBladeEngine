using DualBlade._2D.Physics.Components;
using DualBlade._2D.Physics.Services;
using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using ExampleGame.Components;
using nkast.Aether.Physics2D.Dynamics;

namespace ExampleGame.Entities;

[AddComponent<RenderComponent>]
[AddComponent<CharacterComponent>]
[AddComponent<TransformComponent>]
public partial struct BallEntity : IEntity
{
    public KinematicComponent KinematicComponent { get; }
    public CharacterComponent CharacterComponent { get; }

    public BallEntity(IGameContext context, IPhysicsManager physicsManager)
    {
        InitComponents();
        using var transform = TransformComponentProxy;
        using var renderer = RenderComponentProxy;

        var gameEngine = context.GameEngine;
        renderer.Value.SetSprite(gameEngine.CreateSprite("ball"));

        // Create a physics body
        var body = physicsManager.CreateBody(transform.Value.Position, bodyType: BodyType.Dynamic);
        body.Tag = this;
        body.FixedRotation = true;
        body.Mass = 1;
        body.LinearDamping = 0f;
        var fixture = body.CreateCircle(renderer.Value.Sprite!.Width / 2f, 1);
        fixture.Tag = this;
        fixture.Restitution = 0.01f;
        fixture.Friction = 0f;

        var kinematic = new KinematicComponent
        {
            PhysicsBody = body
        };

        AddComponent(kinematic);
    }
}
