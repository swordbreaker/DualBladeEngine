using Jab;
using DualBlade.Core;
using DualBlade.Core.Services;
using DualBlade._2D.Rendering;
using DualBlade.MyraUi;
using System.Collections.Generic;
using FluidBattle.Scenes;
using DualBlade._2D.Physics;
using FluidBattle.Systems;
using FluidBattle.Services;

namespace FluidBattle;

[ServiceProvider(RootServices = [typeof(IEnumerable<IStartupService>)])]
[Import<ICoreServiceModule>]
[Import<I2DRenderingServiceModule>]
[Import<IMyraUiServiceModule>]
[Import<I2DPhysicsServiceModule>]
[Singleton<MainGame>]
[Transient<MainScene>]
[Transient<MainMenuScene>]
[Transient<DebugColliderSystem>]
[Singleton<ICircleSampler, CircleSampler>]
[Singleton<AiAgent>]
public partial class GameServiceProvider
{
}