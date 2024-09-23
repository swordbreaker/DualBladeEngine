using DualBlade.Core.Services;

namespace DualBlade.Core.Systems;

public class InputSystem(IGameContext gameContext) : BaseSystem(gameContext)
{
    public override void Update(GameTime gameTime)
    {
        GameContext.GameEngine.InputManager.Update();
    }
}
