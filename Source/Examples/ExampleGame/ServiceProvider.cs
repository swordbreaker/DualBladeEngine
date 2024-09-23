using Jab;
using DualBlade.Core;
using DualBlade.Core.Services;
using DualBlade._2D.Physics;
using DualBlade._2D.Rendering;
using ExampleGame.Systems;
using ExampleGame.Scenes;
using DualBlade.MyraUi;

namespace ExampleGame;

[ServiceProvider(RootServices = [typeof(IEnumerable<IStartupService>)])]
[Import<ICoreServiceModule>]
[Import<I2DPhysicsServiceModule>]
[Import<I2DRenderingServiceModule>]
[Import<IMyraUiServiceModule>]
[Singleton<MainGame>]
[Transient<BallSystem>]
[Transient<SpawnSystem>]
[Transient<MainMenuScene>]
[Transient<MainScene>]
public partial class GameServiceProvider
{
}
