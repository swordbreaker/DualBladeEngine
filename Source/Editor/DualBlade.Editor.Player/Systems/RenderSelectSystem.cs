using DualBlade._2D.Rendering.Components;
using DualBlade._2D.Rendering.Extensions;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using DualBlade.Editor.Player.Components;
using Microsoft.Xna.Framework;

namespace DualBlade.Editor.Player.Systems;

internal class RenderSelectSystem(IGameContext gameContext) : ComponentSystem<RenderComponent>(gameContext)
{
    protected override void OnAdded(ref IEntity entity, ref RenderComponent component)
    {
        var selectable = new SelectableComponent
        {
            Rect = CreateRect(ref component, ref entity)
        };
        entity.AddComponent(selectable);
    }

    private Rectangle CreateRect(ref RenderComponent component, ref IEntity entity)
    {
        var size = component.Sprite.Size;
        var transform = entity.Component<TransformComponent>();
        var pos = GameContext.GameEngine.WorldToPixelConverter.WorldPointToPixel(Ecs.AbsolutePosition(entity));
        var scale = Ecs.AbsoluteScale(entity);
        var origin = GameContext.GameEngine.WorldToPixelConverter.WorldSizeToPixel(component.Origin);

        var pixelSize = GameContext.GameEngine.WorldToPixelConverter.WorldSizeToPixel(size * scale);

        var rectPos = new Vector2(pos.X - origin.X, pos.Y - origin.Y);

        return new Rectangle(rectPos.ToPoint(), pixelSize.ToPoint());
    }
}
