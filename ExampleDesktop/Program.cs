using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ExampleGame;
using DualBlade.Core;
using DualBlade._2D.Rendering;
using DualBlade.GumUi;

var host = new HostBuilder()
    .AddGameEngine()
    .AddPhysics2D()
    .Add2DRendering()
    .AddGumUi()
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<MainGame>();
    })
    .Build();

var game = host.Services.GetRequiredService<MainGame>();
game.Run();