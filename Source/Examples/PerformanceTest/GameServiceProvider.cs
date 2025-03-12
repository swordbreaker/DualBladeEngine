using Jab;
using DualBlade.Core;
using DualBlade.Core.Services;
using DualBlade._2D.Rendering;
using DualBlade.MyraUi;
using PerformanceTest.Scenes;
using PerformanceTest;
using System.Collections.Generic;
using PerformanceTest.Systems;
using DualBlade.Core.Systems;

namespace ExampleGame;

[ServiceProvider(RootServices = [typeof(IEnumerable<IStartupService>)])]
[Import<ICoreServiceModule>]
[Import<I2DRenderingServiceModule>]
[Import<IMyraUiServiceModule>]
[Singleton<Game>]
[Transient<MainScene>]
[Transient<MessureUiScene>]
[Transient<ParticleSystem>]
[Transient<ParticleEmitterSystem>]
[Transient<WorldDebugInfoSystem>]
public partial class GameServiceProvider
{
}
