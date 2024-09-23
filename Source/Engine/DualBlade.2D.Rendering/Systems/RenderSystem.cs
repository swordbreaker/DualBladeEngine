using DualBlade._2D.Rendering.Components;
using DualBlade._2D.Rendering.Extensions;
using DualBlade.Core.Components;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;

namespace DualBlade._2D.Rendering.Systems;

public class RenderSystem(IGameContext gameContext) : ComponentSystem<RenderComponent>(gameContext)
{
    private readonly IGameEngine _gameEngine = gameContext.GameEngine;

    protected override void Draw(RenderComponent component, GameTime gameTime)
    {
        var transform = GetTransformComponent(component);
        var position = transform.AbsolutePosition();
        var origin = component.Origin;
        var rotation = MathHelper.ToRadians(transform.AbsoluteRotation());

        if (component.Sprite is null)
        {
            return;
        }

        _gameEngine.Draw(
            component.Sprite.Texture2D,
            position,
            component.Color,
            scale: transform.AbsoluteScale(),
            rotation: rotation,
            origin: origin);
    }

    protected TransformComponent GetTransformComponent(IComponent component) =>
        World.GetComponent<TransformComponent>(component.Entity)!;
}
