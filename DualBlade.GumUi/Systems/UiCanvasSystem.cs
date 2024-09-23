using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using DualBlade.GumUi.Components;
using MonoGameGum.Forms;
using RenderingLibrary;
using System.Diagnostics;

namespace DualBlade.GumUi.Systems;
public class UiCanvasSystem(IGameContext gameContext) : ComponentSystem<UiCanvasComponent>(gameContext)
{
    protected override void Initialize(UiCanvasComponent component)
    {
        base.Initialize(component);
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

    protected override void Update(UiCanvasComponent component, GameTime gameTime)
    {
        base.Update(component, gameTime);
        FormUtils.Update(gameTime, component.Container);
    }

    protected override void OnDestroy(UiCanvasComponent component)
    {
        base.OnDestroy(component);
        component.Container.RemoveFromManagers();
    }
}
