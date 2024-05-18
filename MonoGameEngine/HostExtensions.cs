using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MonoGameEngine.Engine.Physics;
using MonoGameEngine.Engine.Systems;
using MonoGameEngine.Engine.Worlds;

namespace MonoGameEngine;

public static class HostExtensions
{
    public static IHostBuilder AddGameEngine(this IHostBuilder hostBuilder) =>
        hostBuilder.ConfigureServices(context =>
        {
            context.AddSingleton<InputManager>();
            context.AddSingleton<IGameEngineFactory, GameEngineFactory>();
            context.AddSingleton<IWorldFactory, WorldFactory>();
            context.AddSingleton<IPhysicsManager>(new PhysicsManager(Vector2.UnitY * 9.8f));
            context.AddSingleton<ISystemFactory, SystemFactory>();
        });
}
