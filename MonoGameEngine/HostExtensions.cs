using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Xna.Framework;
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
            context.AddSingleton<IPhysicsManager>(new PhysicsManager(new Vector2(0, 9.81f * 25)));
            context.AddSingleton<ISystemFactory, SystemFactory>();
        });
}
