using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ExampleGame.Entities;
using MonoGameEngine.Engine.Systems;
using MonoGameEngine.Engine.Worlds;
using MonoGameEngine.Engine.Services;
using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Dynamics.Contacts;
using System.Diagnostics;
using MonoGameEngine.Engine.Extensions;

namespace ExampleGame.Systems;
public class BallSystem(CameraService cameraService) : EntitySystem<BallEntity>
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
            velocity.X -= 50f * gameTime.DeltaSeconds();
        }
        else if (gameEngine.InputManager.IsKeyPressed(Keys.D))
        {
            velocity.X += 50 * gameTime.DeltaSeconds();
        }
        else
        {
            velocity.X = 0;
        }

        if (gameEngine.InputManager.IsKeyJustPressed(Keys.Space) && entity.CharacterComponent.IsGrounded)
        {
            velocity.Y = -300 * gameTime.DeltaSeconds();
        }

        Debug.WriteLine(velocity);
        body.LinearVelocity = velocity;
        cameraService.Position = entity.Transform.Position;
    }
}
