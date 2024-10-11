using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using DualBlade.Editor.Player.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;

namespace DualBlade.Editor.Player.Systems;

public class SelectableSystem(IGameContext gameContext) : ComponentSystem<SelectableComponent>(gameContext)
{
    private readonly IInputManager input = gameContext.GameEngine.InputManager;

    private Vector2 SelectRectStartPos = Vector2.Zero;
    private Vector2 SelectRectEndPos = Vector2.Zero;
    private bool drawSelectRect = false;

    private Rectangle selectionRect = Rectangle.Empty;

    private readonly List<Action> _drawCalls = new();

    public override void Update(GameTime gameTime)
    {
        HandleSelectionRect();
        base.Update(gameTime);
    }

    protected override void Update(ref SelectableComponent component, ref IEntity entity, GameTime gameTime)
    {
        var shiftPresed = input.IsKeyPressed(Keys.LeftShift);
        var ctrlPressed = input.IsKeyPressed(Keys.LeftControl);

        if (input.IsLeftMouseJustReleased && selectionRect.Size.ToVector2().LengthSquared() > 2)
        {
            component.IsSelected = selectionRect.Contains(component.Rect);
        }
        else if (input.IsLeftMouseJustPressed)
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

    public override void LateDraw(GameTime gameTime)
    {
        GameContext.GameEngine.BeginDraw();
        if (drawSelectRect)
        {
            GameContext.GameEngine.SpriteBatch.FillRectangle(
                selectionRect,
                new Color(Color.Blue, 0.5f));
        }

        foreach (var drawCall in _drawCalls)
        {
            drawCall();
        }
        GameContext.GameEngine.EndDraw();
        _drawCalls.Clear();
    }

    protected override void Draw(SelectableComponent component, IEntity entity, GameTime gameTime)
    {
        base.Draw(component, entity, gameTime);
        if (component.IsSelected)
        {
            _drawCalls.Add(() => GameContext.GameEngine.SpriteBatch.DrawRectangle(component.Rect, Color.Purple, 2));
        }
    }
}
