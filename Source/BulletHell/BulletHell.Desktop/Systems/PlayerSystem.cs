using BulletHell.Desktop.Entities;
using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using Microsoft.Xna.Framework.Input;

namespace BulletHell.Desktop.Systems;

public class PlayerSystem(IGameContext gameContext) : EntitySystem<PlayerEntity>(gameContext)
{
    protected override void Update(ref PlayerEntity entity, GameTime gameTime)
    {
        var input = gameContext.GameEngine.InputManager;

        var move = Vector2.Zero;
        if (input.IsKeyPressed(Keys.W))
            move.Y += 1;
        if (input.IsKeyPressed(Keys.S))
            move.Y -= 1;
        if (input.IsKeyPressed(Keys.A))
            move.X -= 1;
        if (input.IsKeyPressed(Keys.D))
            move.X += 1;

        entity.UpdateComponent<TransformComponent>((c) =>
        {
            c.Position += move * 5f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            return c;
        });

        base.Update(ref entity, gameTime);
    }
}
