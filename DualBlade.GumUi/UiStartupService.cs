using DualBlade.Core.Services;
using Microsoft.Xna.Framework.Input.Touch;
using RenderingLibrary;

namespace DualBlade.GumUi;
internal class UiStartupService : IStartupService
{
    public void OnStart(IGameContext gameContext)
    {
        SystemManagers.Default = new SystemManagers();
        SystemManagers.Default.Initialize(gameContext.GameEngine.GraphicsDeviceManager.GraphicsDevice, fullInstantiation: true);
        FormUtils.InitializeDefaults();

        TouchPanel.DisplayWidth = gameContext.GameEngine.GraphicsDeviceManager.PreferredBackBufferWidth;
        TouchPanel.DisplayHeight = gameContext.GameEngine.GraphicsDeviceManager.PreferredBackBufferHeight;
    }
}