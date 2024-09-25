using DualBlade._2D.Rendering.Components;
using DualBlade._2D.Rendering.Extensions;
using DualBlade.Core.Components;
using DualBlade.Core.Extensions;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;

namespace DualBlade._2D.Rendering.Systems;

public class RenderSystem(IGameContext gameContext) : ComponentSystem<RenderComponent>(gameContext)
{
    private readonly IGameEngine _gameEngine = gameContext.GameEngine;

    protected override void Initialize(RenderComponent component)
    {
        base.Initialize(component);

        if (component.Entity.GetComponent<TransformComponent>().IsNone)
        {
            throw new Exception("RenderComponent must have a TransformComponent");
        }
    }

    public override void Draw(GameTime gameTime)
    {
        _gameEngine.BeginDraw();
        base.Draw(gameTime);
        _gameEngine.EndDraw();
    }

    protected override void Draw(RenderComponent component, GameTime gameTime)
    {
        var transform = GetTransformComponent(component);
        var position = transform.AbsolutePosition();
        var origin = component.Origin;
        var rotation = MathHelper.ToRadians(transform.AbsoluteRotation());
        var scale = transform.AbsoluteScale();

        if (component.Sprite is null)
        {
            return;
        }

        _gameEngine.Draw(
            component.Sprite.Texture2D,
            position,
            component.Color,
            scale: scale,
            rotation: rotation,
            origin: origin);
    }

    protected TransformComponent GetTransformComponent(IComponent component) =>
        World.GetComponent<TransformComponent>(component.Entity)!;
}
