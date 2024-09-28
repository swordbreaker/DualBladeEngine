using DualBlade._2D.Rendering.Components;
using DualBlade._2D.Rendering.Extensions;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;

namespace DualBlade._2D.Rendering.Systems;

public class RenderSystem(IGameContext gameContext) : ComponentSystem<RenderComponent>(gameContext)
{
    private readonly IGameEngine _gameEngine = gameContext.GameEngine;

    protected override void OnAdded(ref RenderComponent component)
    {
        if (Ecs.GetAdjacentComponent<TransformComponent>(component) is null)
        {
            throw new Exception("RenderComponent must have a TransformComponent");
        }
    }

    public override void Draw(GameTime gameTime)
    {
        _gameEngine.BeginDraw();
    }

    public override void AfterDraw(GameTime gameTime)
    {
        _gameEngine.EndDraw();
    }

    protected override void Draw(RenderComponent component, GameTime gameTime)
    {
        var transform = Ecs.GetAdjacentComponent<TransformComponent>(component)!.Value.GetCopy();
        var position = Ecs.AbsolutePosition(transform);
        var origin = component.Origin;
        var rotation = MathHelper.ToRadians(Ecs.AbsoluteRotation(transform));
        var scale = Ecs.AbsoluteScale(transform);

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
}
