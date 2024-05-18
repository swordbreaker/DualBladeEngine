using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ExampleGame.Entities;
using MonoGameEngine.Engine.Systems;
using MonoGameEngine.Engine.Worlds;
using MonoGameEngine.Engine.Services;

namespace ExampleGame.Systems;
public class BallSystem(CameraService cameraService) : EntitySystem<BallEntity>
{
    private const float G = 9.81f * 10;
    private static readonly Vector2 Gravity = new(0, G);

    protected override void Initialize(BallEntity entity, IGameEngine gameEngine)
    {
        //entity.KinematicComponent.PhysicsBody.OnCollision += PhysicsBody_OnCollision;
    }

    protected override void Update(BallEntity entity, GameTime gameTime, IGameEngine gameEngine)
    {
        var jumpVelocity = Vector2.Zero;
        var moveVelocity = Vector2.Zero;

        var body = entity.KinematicComponent.PhysicsBody;

        if (gameEngine.InputManager.IsKeyJustPressed(Keys.Space))
        {
            jumpVelocity += new Vector2(0, -20000);
        }

        if (gameEngine.InputManager.IsKeyPressed(Keys.A))
        {
            moveVelocity += new Vector2(-200, 0);
        }
        else if (gameEngine.InputManager.IsKeyPressed(Keys.D))
        {
            moveVelocity += new Vector2(200, 0);
        }

        entity.KinematicComponent.VerticalVelocity += jumpVelocity + Gravity;
        //var velocity = entity.KinematicComponent.PhysicsBody.LinearVelocity + moveVelocity + jumpVelocity;
        //velocity *= (float)gameTime.ElapsedGameTime.TotalSeconds;
        //var x = Math.Clamp(velocity.X, -100, 100);
        body.LinearVelocity = moveVelocity + entity.KinematicComponent.VerticalVelocity;

        cameraService.Position = entity.Transform.Position;
    }
}
 