using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ExampleGame.Entities;
using MonoGameEngine.Engine.Systems;
using MonoGameEngine.Engine.Worlds;
using MonoGameEngine.Engine.Services;
using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Dynamics.Contacts;
using MonoGameEngine.Engine.Extensions;

namespace ExampleGame.Systems;

public class BallSystem(ICameraService cameraService) : EntitySystem<BallEntity>
{
    protected override void Initialize(BallEntity entity, IGameEngine gameEngine)
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

    protected override void Update(BallEntity entity, GameTime gameTime, IGameEngine gameEngine)
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

        if (gameEngine.InputManager.IsKeyJustPressed(Keys.Space) && entity.CharacterComponent.IsGrounded)
        {
            velocity.Y = jumpForce * gameTime.DeltaSeconds();
            entity.CharacterComponent.IsJumping = true;
            gameEngine.PhysicsManager.Gravity = new Vector2(0, -5);
        }

        if (!gameEngine.InputManager.IsKeyPressed(Keys.Space) && entity.CharacterComponent.IsJumping)
        {
            entity.CharacterComponent.IsJumping = false;
            gameEngine.PhysicsManager.Gravity = new Vector2(0, -20);
        }

        if (velocity.Y < 0 && entity.CharacterComponent.IsJumping)
        {
            entity.CharacterComponent.IsJumping = false;
            gameEngine.PhysicsManager.Gravity = new Vector2(0, -20);
        }

        body.LinearVelocity = velocity;
        cameraService.Position = entity.Transform.Position;
    }
}
