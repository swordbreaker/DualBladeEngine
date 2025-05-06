using Jab;
using DualBlade.Core;
using DualBlade.Core.Services;
using DualBlade._2D.Rendering;
using DualBlade.MyraUi;
using DualBlade._2D.BladePhysics;
using System.Collections.Generic;
using DualBlade._2D.BladePhysics.Services;
using BulletHell.Desktop;
using BulletHell.Desktop.Scences;

namespace ExampleGame;

[ServiceProvider(RootServices = [typeof(IEnumerable<IStartupService>)])]
[Import<ICoreServiceModule>]
[Import<I2DBladePhysicsServiceModule>]
[Import<I2DRenderingServiceModule>]
[Import<IMyraUiServiceModule>]
[Singleton<MainGame>]
[Singleton<IPhysicsSettings, PhysicSettings>]
[Transient<MainScene>]
public partial class GameServiceProvider
{
}
