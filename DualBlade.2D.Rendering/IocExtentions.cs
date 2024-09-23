using DualBlade.Core.Factories;
using DualBlade.Core.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DualBlade._2D.Rendering;

public static class IocExtensions
{
    public static IHostBuilder Add2DRendering(this IHostBuilder hostBuilder) =>
        hostBuilder.ConfigureServices(context =>
        {
            context.AddSingleton<ISpriteFactory, SpriteFactory>();
            context.AddSingleton<ICameraServiceFactory, CameraServiceFactory>();
            context.AddSingleton<IWorldToPixelConverterFactory, WorldToPixelConverterFactory>();
        });
}
