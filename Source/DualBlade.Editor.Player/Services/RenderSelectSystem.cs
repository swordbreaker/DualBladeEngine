using DualBlade._2D.Rendering.Components;
using DualBlade._2D.Rendering.Extensions;
using DualBlade.Core.Extensions;
using DualBlade.Core.Rendering;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using DualBlade.Editor.Player.Components;
using FunctionalMonads.Monads.MaybeMonad;
using Microsoft.Xna.Framework;
using System;

namespace DualBlade.Editor.Player.Services;

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
        var transform = component.Entity.GetComponent<TransformComponent>().SomeOrProvided(() => throw new Exception("RenderComponent must have a TransformComponent"));
        var pos = transform.AbsolutePosition();
        var origin = component.Origin;

        var rectPos = new Vector2(pos.X - origin.X, pos.Y - origin.Y);

        return new Rectangle(rectPos.ToPoint(), size.ToPoint());
    }
}
