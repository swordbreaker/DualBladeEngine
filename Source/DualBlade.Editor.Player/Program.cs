using DualBlade._2D.Physics;
using DualBlade._2D.Rendering;
using DualBlade.Core;
using DualBlade.Core.Services;
using DualBlade.Editor.Player.Services;
using DualBlade.MyraUi;
using Editor;
using Editor.Systems;
using Jab;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

internal class Program
{
    private static void Main(string[] args)
    {
        var provider = new ServiceProvider();
        var game = provider.GetRequiredService<EditorGame>();
        game.Run();
    }
}

[ServiceProvider(RootServices = [typeof(IEnumerable<IStartupService>)])]
[Import<ICoreServiceModule>]
[Import<I2DRenderingServiceModule>]
[Import<I2DPhysicsServiceModule>]
[Import<IMyraUiServiceModule>]
[Singleton<EditorGame>]
[Singleton<SystemProvider>]
[Transient<CameraSystem>]
[Transient<RenderSelectSystem>]
[Transient<SelectableSystem>]
internal partial class ServiceProvider
{
}


//var host = new HostBuilder()
//    .AddGameEngine()
//    .AddPhysics2D()
//    .Add2DRendering()
//    .AddGumUi()
//    .ConfigureServices((context, services) =>
//    {
//        services.AddSingleton<EditorGame>();
//    })
//    .Build();

//var game = host.Services.GetRequiredService<EditorGame>();
//game.Run();