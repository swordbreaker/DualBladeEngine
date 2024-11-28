using DualBlade._2D.Rendering.Components;
using DualBlade._2D.Rendering.Extensions;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using Microsoft.Xna.Framework.Graphics;

namespace DualBlade._2D.Rendering.Systems;

public class RenderSystem(IGameContext gameContext) : ComponentSystem<RenderComponent, TransformComponent>(gameContext)
{
    private record RenderData(
        Texture2D Texture2D,
        Vector2 AbsolutePosition,
        Vector2 AbsoluteScale,
        float AbsoluteRotation,
        Color Color,
        Vector2 Origin);

    private readonly IGameEngine _gameEngine = gameContext.GameEngine;
    private readonly List<RenderData> collectedRenderData = new();

    public override void Draw(GameTime gameTime)
    {
        collectedRenderData.Clear();
    }

    public override void LateDraw(GameTime gameTime)
    {
        _gameEngine.BeginDraw();

        foreach (var renderData in collectedRenderData)
        {
            _gameEngine.Draw(
                renderData.Texture2D,
                renderData.AbsolutePosition,
                renderData.Color,
                scale: renderData.AbsoluteScale,
                rotation: renderData.AbsoluteRotation,
                origin: renderData.Origin);
        }

        _gameEngine.EndDraw();
    }

    protected override void Draw(RenderComponent render, TransformComponent transform, IEntity entity,
        GameTime gameTime)
    {
        var position = Ecs.AbsolutePosition(entity);
        var origin = render.Origin;
        var rotation = MathHelper.ToRadians(Ecs.AbsoluteRotation(entity));
        var scale = Ecs.AbsoluteScale(entity);

        if (render.Sprite is null)
        {
            return;
        }

        collectedRenderData.Add(new RenderData(
            render.Sprite.Texture2D,
            position,
            scale,
            rotation,
            render.Color,
            origin));
    }
}