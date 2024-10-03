using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using DualBlade.GumUi.Components;
using RenderingLibrary;

namespace DualBlade.GumUi.Systems;
public class UiCanvasSystem(IGameContext gameContext) : ComponentSystem<UiCanvasComponent>(gameContext)
{
    protected override void OnAdded(ref IEntity entity, ref UiCanvasComponent component)
    {
        component.Container.AddToManagers();
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
        SystemManagers.Default.Draw();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        SystemManagers.Default.Activity(gameTime.ElapsedGameTime.Milliseconds);
    }

    protected override void Update(ref UiCanvasComponent component, ref IEntity entity, GameTime gameTime)
    {
        FormUtils.Update(gameTime, component.Container);
    }

    protected override void OnDestroy(UiCanvasComponent component, IEntity entity)
    {
        component.Container.RemoveFromManagers();
    }
}
