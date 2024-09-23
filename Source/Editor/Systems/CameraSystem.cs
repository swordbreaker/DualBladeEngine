using DualBlade.Core.Extensions;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Editor.Systems;
public class CameraSystem(IGameContext context) : BaseSystem(context)
{
    private readonly IGameEngine gameEngine = context.GameEngine;

    public override void Update(GameTime gameTime)
    {
        var zoom = gameEngine.InputManager.ScrollWheelDelta * 0.0005f;
        gameEngine.CameraService.Zoom += zoom;

        var left = gameEngine.InputManager.IsKeyPressed(Keys.A) ? -1 : 0;
        var right = gameEngine.InputManager.IsKeyPressed(Keys.D) ? 1 : 0;
        var up = gameEngine.InputManager.IsKeyPressed(Keys.W) ? 1 : 0;
        var down = gameEngine.InputManager.IsKeyPressed(Keys.S) ? -1 : 0;

        var move = new Vector2(right + left, up + down) * gameTime.DeltaSeconds() * 50;

        gameEngine.CameraService.Position += move;
        Debug.WriteLine(zoom);

        if (gameEngine.InputManager.IsKeyPressed(Keys.Space))
        {
            gameEngine.CameraService.Zoom = 1;
            gameEngine.CameraService.Position = Vector2.Zero;
        }
    }
}
