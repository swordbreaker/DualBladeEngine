using Microsoft.Xna.Framework;
using TestMonoGamesProject.Engine.Worlds;

namespace TestMonoGamesProject.Engine.Systems;

public class InputSystem : ISystem
{
    public void Draw(GameTime gameTime, IGameEngine gameEngine) { }

    public void Initialize(IGameEngine gameEngine) { }

    public void Update(GameTime gameTime, IGameEngine gameEngine)
    {
        gameEngine.InputManager.Update();
    }
}
