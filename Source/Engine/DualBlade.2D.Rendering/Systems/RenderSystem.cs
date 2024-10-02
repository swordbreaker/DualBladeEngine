using DualBlade._2D.Rendering.Components;
using DualBlade._2D.Rendering.Extensions;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;

namespace DualBlade._2D.Rendering.Systems;

public class RenderSystem(IGameContext gameContext) : ComponentSystem<RenderComponent, TransformComponent>(gameContext)
{
    private readonly IGameEngine _gameEngine = gameContext.GameEngine;

    public override void Draw(GameTime gameTime)
    {
        _gameEngine.BeginDraw();
    }

    public override void LateDraw(GameTime gameTime)
    {
        _gameEngine.EndDraw();
    }

    protected override void Draw(RenderComponent render, TransformComponent transform, IEntity entity, GameTime gameTime)
    {
        var position = Ecs.AbsolutePosition(entity);
        var origin = render.Origin;
        var rotation = MathHelper.ToRadians(Ecs.AbsoluteRotation(entity));
        var scale = Ecs.AbsoluteScale(entity);

        if (render.Sprite is null)
        {
            return;
        }

        _gameEngine.Draw(
            render.Sprite.Texture2D,
            position,
            render.Color,
            scale: scale,
            rotation: rotation,
            origin: origin);
    }
}
