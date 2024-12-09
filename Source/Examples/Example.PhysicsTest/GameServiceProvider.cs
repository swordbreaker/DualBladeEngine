using DualBlade.Core.Services;
using Jab;
using DualBlade.Core;
using DualBlade._2D.Rendering;
using DualBlade.MyraUi;
using DualBlade._2D.BladePhysics;
using Example.PhysicsTest.Scences;
using DualBlade._2D.BladePhysics.Systems;

namespace Example.PhysicsTest;

[ServiceProvider(RootServices = [typeof(IEnumerable<IStartupService>)])]
[Import<ICoreServiceModule>]
[Import<I2DRenderingServiceModule>]
[Import<IMyraUiServiceModule>]
[Import<I2DBladePhysicsServiceModule>]
[Singleton<MainGame>]
[Transient<TwoCirclesScene>]
[Transient<DebugColliderSystem>]
[Singleton<DualBlade._2D.BladePhysics.Services.IPhysicsSettings, PhysicsSettings>]
public partial class GameServiceProvider
{
}