using DualBlade.Core.Services;
using MonoGameGum.Forms;
using RenderingLibrary;

namespace DualBlade.GumUi;
internal class UiStartupService : IStartupService
{
    public void OnStart(IGameContext gameContext)
    {
        SystemManagers.Default = new SystemManagers();
        SystemManagers.Default.Initialize(gameContext.GameEngine.GraphicsDeviceManager.GraphicsDevice, fullInstantiation: true);
        FormsUtilities.InitializeDefaults();
    }
}