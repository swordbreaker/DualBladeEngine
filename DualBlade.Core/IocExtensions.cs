using DualBlade.Core.Factories;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DualBlade.Core;

public static class IocExtensions
{
    public static IHostBuilder AddGameEngine(this IHostBuilder hostBuilder) =>
        hostBuilder.ConfigureServices(context =>
        {
            context.AddSingleton<IInputManager, InputManager>();
            context.AddSingleton<IGameEngineFactory, GameEngineFactory>();
            context.AddSingleton<IWorldFactory, WorldFactory>();
            context.AddSingleton<ISystemFactory, SystemFactory>();
            context.AddSingleton<IEntityFactory, EntityFactory>();
            context.AddSingleton<ISceneManagerFactory, SceneManagerFactory>();
            context.AddSingleton<IJobQueue, JobQueue>();
            context.AddSingleton<IGameContext, GameContext>();
            context.AddSingleton<IGameCreationContext, GameCreationContext>();
        });
}
