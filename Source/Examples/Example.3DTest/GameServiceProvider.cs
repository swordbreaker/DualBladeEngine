using System.Collections.Generic;
using DualBlade.Core.Services;
using Jab;
using DualBlade.Core;
using DualBlade._2D.Rendering;
using DualBlade.MyraUi;

namespace Example.PhysicsTest;

[ServiceProvider(RootServices = [typeof(IEnumerable<IStartupService>)])]
[Import<ICoreServiceModule>]
[Import<ICamera3DServiceModule>]
[Import<I3DRenderingServiceModule>]
[Import<IMyraUiServiceModule>]
[Singleton<MainGame>]
public partial class GameServiceProvider
{
}