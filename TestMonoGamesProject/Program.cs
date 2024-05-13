using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestMonoGamesProject;
using TestMonoGamesProject.Engine.Physics;
using TestMonoGamesProject.Engine.World;
using TestMonoGamesProject.Engine.Worlds;

var host = new HostBuilder()
    .ConfigureServices(host =>
    {
        host.AddSingleton<InputManager>();
        host.AddSingleton<IGameEngineFactory, GameEngineFactory>();
        host.AddSingleton<WorldFactory>();
        host.AddSingleton<MainGame>();
        host.AddSingleton<IPhysicsManager, PhysicsManager>();
        host.AddSingleton<IColliderFactory, ColliderFactory>();
    })
    .Build();

host.Start();

var game = host.Services.GetRequiredService<MainGame>();
game.Run();
