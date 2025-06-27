using Jab;
using DualBlade.Core;
using DualBlade.Core.Services;
using DualBlade._2D.Rendering;
using DualBlade.MyraUi;
using DualBlade._2D.BladePhysics;
using System.Collections.Generic;
using DualBlade._2D.BladePhysics.Services;
using BulletHell.Desktop.Scences;
using BulletHell.Desktop.Systems;
using BulletHell.Desktop.Services;

namespace BulletHell.Desktop;

[ServiceProvider(RootServices = [typeof(IEnumerable<IStartupService>)])]
[Import<ICoreServiceModule>]
[Import<I2DBladePhysicsServiceModule>]
[Import<I2DRenderingServiceModule>]
[Import<IMyraUiServiceModule>]
[Singleton<MainGame>]
[Singleton<IPhysicsSettings, PhysicSettings>]
[Singleton<MusicContext, MusicContext>]
[Transient<MainScene>]
[Transient<MoveSystem>]
[Transient<DestroyOnScreenBoundSystem>]
public partial class GameServiceProvider
{
}
