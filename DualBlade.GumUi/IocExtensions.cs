using DualBlade.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DualBlade.GumUi;

public static class IocExtensions
{
    public static IHostBuilder AddGumUi(this IHostBuilder hostBuilder) =>
        hostBuilder.ConfigureServices(context =>
        {
            context.AddSingleton<IStartupService, UiStartupService>();
        });
}