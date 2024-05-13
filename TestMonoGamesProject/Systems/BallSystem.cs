using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using TestMonoGamesProject.Engine.Systems;
using TestMonoGamesProject.Engine.Worlds;
using TestMonoGamesProject.Entities;

namespace TestMonoGamesProject.Systems;
public class BallSystem : EntitySystem<BallEntity>
{
    protected override void Update(BallEntity entity, GameTime gameTime, IGameEngine gameEngine)
    {
        var jumpVelocity = Vector2.Zero;
        var moveVelocity = Vector2.Zero;

        if (gameEngine.InputManager.IsKeyJustPressed(Keys.Space))
        {
            jumpVelocity += new Vector2(0, -1000);
        }

        if (gameEngine.InputManager.IsKeyPressed(Keys.A))
        {
            moveVelocity += new Vector2(-200, 0);
        }
        else if (gameEngine.InputManager.IsKeyPressed(Keys.D))
        {
            moveVelocity += new Vector2(200, 0);
        }
        else
        {
            moveVelocity = Vector2.Zero;
        }

        entity.KinematicComponent.KinematicVelocity += jumpVelocity;
        //moveVelocity = Vector2.Clamp(moveVelocity, new Vector2(-50, -50), new Vector2(50, 50));
        entity.KinematicComponent.Velocity = moveVelocity;
    }
}
