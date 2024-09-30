using DualBlade.Core.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;

namespace DualBlade.Core.Systems;

public class FpsDisplaySystem(IGameContext gameContext) : ComponentSystem<FpsDisplayComponent>(gameContext)
{
    protected override void Update(ref FpsDisplayComponent component, ref IEntity entity, GameTime gameTime)
    {
        component.ElapsedUpdateTime += gameTime.ElapsedGameTime;

        if (component.ElapsedUpdateTime > TimeSpan.FromSeconds(1))
        {
            component.ElapsedUpdateTime -= TimeSpan.FromSeconds(1);
            component.UpdateRate = component.FrameCounter;
            component.UpdateCounter = 0;
        }
    }

    protected override void Draw(FpsDisplayComponent component, IEntity entity, GameTime gameTime)
    {
        component.ElapsedFrameTime += gameTime.ElapsedGameTime;

        if (component.ElapsedFrameTime > TimeSpan.FromSeconds(1))
        {
            component.ElapsedFrameTime -= TimeSpan.FromSeconds(1);
            component.FrameRate = component.FrameCounter;
            component.FrameCounter = 0;
        }

        component.FrameCounter++;
        entity.UpdateComponent(component);
        World.UpdateEntity(entity);

        var displayText = $"FPS: {component.FrameRate} Update: {component.UpdateRate}";

        GameContext.GameEngine.SpriteBatch!.Begin();
        GameContext.GameEngine.SpriteBatch.DrawString(component.Font, displayText, component.Position, component.FontColor);
        GameContext.GameEngine.SpriteBatch.End();
    }
}
