using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Xna.Framework;
using TestMonoGamesProject.Engine.Physics;
using TestMonoGamesProject.Engine.World;
using TestMonoGamesProject.Engine.Worlds;

namespace TestMonoGamesProject;

public static class HostExtensions
{
    public static IHostBuilder AddGameEngine(this IHostBuilder hostBuilder) =>
        hostBuilder.ConfigureServices(context =>
        {
            context.AddSingleton<InputManager>();
            context.AddSingleton<IGameEngineFactory, GameEngineFactory>();
            context.AddSingleton<WorldFactory>();
            context.AddSingleton(new PhysicsManager(new Vector2(0, 9.81f * 25)));
            context.AddSingleton<IColliderFactory, ColliderFactory>();
        });
}
