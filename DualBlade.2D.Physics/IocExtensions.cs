using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services;

namespace DualBlade.Core;

public static class IocExtensions
{
    public static IHostBuilder AddPhysics2D(this IHostBuilder hostBuilder) =>
        hostBuilder.ConfigureServices(context =>
        {
            context.AddSingleton<IPhysicsManager, PhysicsManager>();
        });
}
