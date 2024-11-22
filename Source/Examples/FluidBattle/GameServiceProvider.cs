using Jab;
using DualBlade.Core;
using DualBlade.Core.Services;
using DualBlade._2D.Rendering;
using DualBlade.MyraUi;
using System.Collections.Generic;
using FluidBattle.Scenes;
using FluidBattle.Services;
using FluidBattle.Factories;
using DualBlade._2D.BladePhysics;
using DualBlade._2D.BladePhysics.Services;
using DualBlade._2D.BladePhysics.Systems;

namespace FluidBattle;

[ServiceProvider(RootServices = [typeof(IEnumerable<IStartupService>)])]
[Import<ICoreServiceModule>]
[Import<I2DRenderingServiceModule>]
[Import<IMyraUiServiceModule>]
[Import<I2DBladePhysicsServiceModule>]
[Singleton<MainGame>]
[Transient<MainScene>]
[Transient<MainMenuScene>]
[Transient<DebugColliderSystem>]
[Singleton<ICircleSampler, CircleSampler>]
[Singleton<AiAgent>]
[Singleton<FluidFactory>]
[Singleton<IPhysicsSettings, PhysicsSettings>]
public partial class GameServiceProvider
{
}