using DualBlade._2D.Rendering.Components;
using DualBlade._2D.Rendering.Extensions;
using DualBlade.Core.Extensions;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using DualBlade.Editor.Player.Components;
using FunctionalMonads.Monads.MaybeMonad;
using Microsoft.Xna.Framework;
using System;

namespace DualBlade.Editor.Player.Systems;

internal class RenderSelectSystem(IGameContext gameContext) : ComponentSystem<RenderComponent>(gameContext)
{

    protected override void Initialize(RenderComponent component)
    {
        base.Initialize(component);
        var selectable = component.Entity.AddComponent<SelectableComponent>();
        selectable.Rect = CreateRect(component);
    }

    protected override void Update(RenderComponent component, GameTime gameTime)
    {
        base.Update(component, gameTime);

    }

    private Rectangle CreateRect(RenderComponent component)
    {
        var size = component.Sprite.Size;
        var transform = component.Entity.GetComponent<TransformComponent>();
        var pos = GameContext.GameEngine.WorldToPixelConverter.WorldPointToPixel(transform.AbsolutePosition());
        var scale = transform.AbsoluteScale();
        var origin = GameContext.GameEngine.WorldToPixelConverter.WorldSizeToPixel(component.Origin);

        var pixelSize = GameContext.GameEngine.WorldToPixelConverter.WorldSizeToPixel(size * scale);

        var rectPos = new Vector2(pos.X - origin.X, pos.Y - origin.Y);

        return new Rectangle(rectPos.ToPoint(), pixelSize.ToPoint());
    }
}
