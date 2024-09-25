using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using DualBlade.Editor.Player.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace DualBlade.Editor.Player.Services;

public class SelectableSystem(IGameContext gameContext) : ComponentSystem<SelectableComponent>(gameContext)
{
    private readonly IInputManager input = gameContext.GameEngine.InputManager;


    protected override void Update(SelectableComponent component, GameTime gameTime)
    {
        base.Update(component, gameTime);
        if (input.IsLeftMouseJustPressed && component.Rect.Contains(input.MousePos))
        {
            component.IsSelected = !component.IsSelected;
        }
    }

    public override void Draw(GameTime gameTime)
    {
        GameContext.GameEngine.BeginDraw();
        base.Draw(gameTime);
        GameContext.GameEngine.EndDraw();
    }

    protected override void Draw(SelectableComponent component, GameTime gameTime)
    {
        base.Draw(component, gameTime);
        GameContext.GameEngine.SpriteBatch.DrawRectangle(component.Rect, Color.Purple, 2);
    }
}
