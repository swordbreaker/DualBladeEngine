//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using System.Numerics;
//using TestMonoGamesProject;
//using TestMonoGamesProject.Engine.Physics;
//using TestMonoGamesProject.Engine.World;
//using TestMonoGamesProject.Engine.Worlds;

//var host = new HostBuilder()
//    .ConfigureServices(host =>
//    {
//        host.AddSingleton<InputManager>();
//        host.AddSingleton<IGameEngineFactory, GameEngineFactory>();
//        host.AddSingleton<WorldFactory>();
//        host.AddSingleton<MainGame>();
//        host.AddSingleton(new PhysicsManager(new Vector2(0, 9.81f * 25)));
//        host.AddSingleton<IColliderFactory, ColliderFactory>();
//    })
//    .Build();

//host.Start();

//var game = host.Services.GetRequiredService<MainGame>();
//game.Run();
