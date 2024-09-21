using MonoGameEngine.Engine.Systems;
using MonoGameEngine.Engine.Worlds;
using MonoGameEngine.Ui.Components;
using MonoGameGum.Forms;
using RenderingLibrary;

namespace MonoGameEngine.Ui.Systems;
public class UiCanvasSystem : ComponentSystem<UiCanvasComponent>
{
    protected override void Initialize(UiCanvasComponent component, IGameEngine gameEngine)
    {
        base.Initialize(component, gameEngine);
        component.Container.AddToManagers();
    }

    public override void Draw(GameTime gameTime, IGameEngine gameEngine)
    {
        base.Draw(gameTime, gameEngine);
        SystemManagers.Default.Draw();
    }

    public override void Update(GameTime gameTime, IGameEngine gameEngine)
    {
        base.Update(gameTime, gameEngine);
        SystemManagers.Default.Activity(gameTime.ElapsedGameTime.Milliseconds);
    }

    protected override void Update(UiCanvasComponent component, GameTime gameTime, IGameEngine gameEngine)
    {
        base.Update(component, gameTime, gameEngine);
        FormsUtilities.Update(gameTime, component.Container);
    }
}
