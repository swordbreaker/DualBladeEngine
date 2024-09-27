using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using DualBlade.Editor.Player.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;

namespace DualBlade.Editor.Player.Systems;

public class SelectableSystem(IGameContext gameContext) : ComponentSystem<SelectableComponent>(gameContext)
{
    private readonly IInputManager input = gameContext.GameEngine.InputManager;

    private Vector2 SelectRectStartPos = Vector2.Zero;
    private Vector2 SelectRectEndPos = Vector2.Zero;
    private bool drawSelectRect = false;

    private Rectangle selectionRect = Rectangle.Empty;

    public override void Update(GameTime gameTime)
    {
        HandleSelectionRect();
        base.Update(gameTime);
    }

    protected override void Update(SelectableComponent component, GameTime gameTime)
    {
        base.Update(component, gameTime);

        var shiftPresed = input.IsKeyPressed(Keys.LeftShift);
        var ctrlPressed = input.IsKeyPressed(Keys.LeftControl);

        if (input.IsLeftMouseJustPressed)
        {
            if (component.Rect.Contains(input.MousePos))
            {
                component.IsSelected = !shiftPresed;
            }
            else if (!ctrlPressed)
            {
                component.IsSelected = false;
            }
        }
    }

    private void HandleSelectionRect()
    {
        if (input.IsLeftMouseJustPressed)
        {
            SelectRectStartPos = input.MousePos;
        }

        if (input.IsLeftMousePressed)
        {
            SelectRectEndPos = input.MousePos;
        }

        drawSelectRect = input.IsLeftMousePressed;

        if (drawSelectRect)
        {
            selectionRect = CreateSelectionRect(SelectRectStartPos, SelectRectEndPos);
        }

        if (input.IsLeftMouseJustReleased && selectionRect.Size.ToVector2().LengthSquared() > 2)
        {
            drawSelectRect = false;

            foreach (var component in World.GetComponents<SelectableComponent>())
            {
                component.IsSelected = selectionRect.Contains(component.Rect);
            }
        }
    }

    private Rectangle CreateSelectionRect(Vector2 start, Vector2 end)
    {
        var min = new Vector2(
            MathF.Min(start.X, end.X),
            MathF.Min(start.Y, end.Y));

        var max = new Vector2(
            MathF.Max(start.X, end.X),
            MathF.Max(start.Y, end.Y));

        return new Rectangle(min.ToPoint(), (max - min).ToPoint());
    }

    public override void Draw(GameTime gameTime)
    {
        GameContext.GameEngine.BeginDraw();
        base.Draw(gameTime);

        if (drawSelectRect)
        {
            GameContext.GameEngine.SpriteBatch.FillRectangle(
                selectionRect,
                new Color(Color.Blue, 0.5f));
        }
        GameContext.GameEngine.EndDraw();
    }

    protected override void Draw(SelectableComponent component, GameTime gameTime)
    {
        base.Draw(component, gameTime);
        if (component.IsSelected)
        {
            GameContext.GameEngine.SpriteBatch.DrawRectangle(component.Rect, Color.Purple, 2);
        }
    }
}
