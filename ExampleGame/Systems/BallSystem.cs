using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ExampleGame.Entities;
using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Dynamics.Contacts;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using DualBlade.Core.Extensions;
using Services;

namespace ExampleGame.Systems;

public class BallSystem(IGameContext context, IPhysicsManager physicsManager) : EntitySystem<BallEntity>(context)
{
    private readonly IGameEngine gameEngine = context.GameEngine;
    private ICameraService CameraService => gameEngine.CameraService;

    protected override void Initialize(BallEntity entity)
    {
        entity.KinematicComponent.PhysicsBody.OnCollision += PhysicsBody_OnCollision;
        entity.KinematicComponent.PhysicsBody.OnSeparation += PhysicsBody_OnSeparation;
    }

    private bool PhysicsBody_OnCollision(Fixture sender, Fixture other, Contact contact)
    {
        if (sender.Tag is BallEntity ballEntity && other.Tag is GroundEntity)
        {
            ballEntity.CharacterComponent.IsGrounded = true;
        }

        return true;
    }

    private void PhysicsBody_OnSeparation(Fixture sender, Fixture other, Contact contact)
    {
        if (sender.Tag is BallEntity ballEntity && other.Tag is GroundEntity)
        {
            ballEntity.CharacterComponent.IsGrounded = false;
        }
    }

    protected override void Update(BallEntity entity, GameTime gameTime)
    {
        var body = entity.KinematicComponent.PhysicsBody;

        var velocity = body.LinearVelocity;

        if (gameEngine.InputManager.IsKeyPressed(Keys.A))
        {
            velocity.X = MathF.Min(0, velocity.X);
            velocity.X -= 20 * gameTime.DeltaSeconds();
        }
        else if (gameEngine.InputManager.IsKeyPressed(Keys.D))
        {
            velocity.X = MathF.Max(0, velocity.X);
            velocity.X += 20 * gameTime.DeltaSeconds();
        }
        else
        {
            velocity.X = 0;
        }

        var jumpForce = 300;

        var jumpPressed = gameEngine.InputManager.TouchState.Count > 0 || gameEngine.InputManager.IsKeyJustPressed(Keys.Space);

        if (jumpPressed && entity.CharacterComponent.IsGrounded)
        {
            velocity.Y = jumpForce * gameTime.DeltaSeconds();
            entity.CharacterComponent.IsJumping = true;
            physicsManager.Gravity = new Vector2(0, -5);
        }

        if (!gameEngine.InputManager.IsKeyPressed(Keys.Space) && entity.CharacterComponent.IsJumping)
        {
            entity.CharacterComponent.IsJumping = false;
            physicsManager.Gravity = new Vector2(0, -20);
        }

        if (velocity.Y < 0 && entity.CharacterComponent.IsJumping)
        {
            entity.CharacterComponent.IsJumping = false;
            physicsManager.Gravity = new Vector2(0, -20);
        }

        body.LinearVelocity = velocity;
        CameraService.Position = entity.Transform.Position;
    }
}
