using Editor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MonoGameEngine;

var host = new HostBuilder()
    .AddGameEngine()
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<EditorGame>();
    })
    .Build();

var game = host.Services.GetRequiredService<EditorGame>();
game.Run();