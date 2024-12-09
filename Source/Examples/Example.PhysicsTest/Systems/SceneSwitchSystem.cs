using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using Example.PhysicsTest.Scences;
using Microsoft.Xna.Framework.Input;

namespace Example.PhysicsTest.Systems;

public class SceneSwitchSystem(IGameContext gameContext) : BaseSystem(gameContext)
{
    private readonly IInputManager inputManager = gameContext.GameEngine.InputManager;
    private readonly ISceneManager sceneManager = gameContext.SceneManager;

    public override void Update(GameTime gameTime)
    {
        if (inputManager.IsKeyJustPressed(Keys.D1))
        {
            sceneManager.AddSceneExclusively(new TwoCirclesScene(GameContext));
        }
        else if (inputManager.IsKeyJustPressed(Keys.D2))
        {
            sceneManager.AddSceneExclusively(new TwoSquaresScene(GameContext));
        }
        else if (inputManager.IsKeyJustPressed(Keys.D3))
        {
            sceneManager.AddSceneExclusively(new CircleAndSquareScene(GameContext));
        }
    }
}
