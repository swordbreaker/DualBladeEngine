using DualBlade.Core.Components;
using DualBlade.Core.Services;

namespace DualBlade.Core.Systems;

public class FpsDisplaySystem(IGameContext gameContext) : ComponentSystem<FpsDisplayComponent>(gameContext)
{
    protected override void Update(FpsDisplayComponent component, GameTime gameTime)
    {
        base.Update(component, gameTime);
        component.ElapsedUpdateTime += gameTime.ElapsedGameTime;

        if (component.ElapsedUpdateTime > TimeSpan.FromSeconds(1))
        {
            component.ElapsedUpdateTime -= TimeSpan.FromSeconds(1);
            component.UpdateRate = component.FrameCounter;
            component.UpdateCounter = 0;
        }
    }

    protected override void Draw(FpsDisplayComponent component, GameTime gameTime)
    {
        base.Draw(component, gameTime);

        component.ElapsedFrameTime += gameTime.ElapsedGameTime;

        if (component.ElapsedFrameTime > TimeSpan.FromSeconds(1))
        {
            component.ElapsedFrameTime -= TimeSpan.FromSeconds(1);
            component.FrameRate = component.FrameCounter;
            component.FrameCounter = 0;
        }

        component.FrameCounter++;
        var displayText = $"FPS: {component.FrameRate} Update: {component.UpdateRate}";

        GameContext.GameEngine.SpriteBatch.Begin();
        GameContext.GameEngine.SpriteBatch!.DrawString(component.Font, displayText, component.Position, component.FontColor);
        GameContext.GameEngine.SpriteBatch.End();
    }
}
