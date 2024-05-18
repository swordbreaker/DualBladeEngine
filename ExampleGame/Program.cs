using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ExampleGame;
using MonoGameEngine;

var host = new HostBuilder()
    .AddGameEngine()
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<MainGame>();
    })
    .Build(); 

var game = host.Services.GetRequiredService<MainGame>();
game.Run();