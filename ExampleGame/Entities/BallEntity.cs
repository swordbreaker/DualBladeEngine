using Components;
using DualBlade._2D.Rendering.Entities;
using DualBlade.Core.Services;
using ExampleGame.Components;
using nkast.Aether.Physics2D.Dynamics;
using Services;
using System.Numerics;

namespace ExampleGame.Entities;

public class BallEntity : SpriteEntity
{
    public KinematicComponent KinematicComponent { get; }
    public CharacterComponent CharacterComponent { get; }

    public BallEntity(IGameContext context, IPhysicsManager physicsManager)
    {
        var gameEngine = context.GameEngine;

        KinematicComponent = AddComponent<KinematicComponent>();
        CharacterComponent = AddComponent<CharacterComponent>();

        Transform!.Position = Vector2.Zero;

        Renderer.SetSprite(gameEngine.CreateSprite("ball"));

        // Create a physics body
        var body = physicsManager.CreateBody(Transform.Position, bodyType: BodyType.Dynamic);
        body.Tag = this;
        body.FixedRotation = true;
        body.Mass = 1;
        body.LinearDamping = 0f;
        var fixture = body.CreateCircle(Renderer.Sprite!.Width / 2f, 1);
        fixture.Tag = this;
        fixture.Restitution = 0.01f;
        fixture.Friction = 0f;

        KinematicComponent.PhysicsBody = body;
    }
}
