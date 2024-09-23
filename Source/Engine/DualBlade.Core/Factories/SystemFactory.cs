using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using Microsoft.Extensions.DependencyInjection;

namespace DualBlade.Core.Factories;

public sealed class SystemFactory(IGameContext gameContext) : ISystemFactory
{
    public delegate object Resolver(Type type);

    public TSystem Create<TSystem>() where TSystem : ISystem =>
        gameContext.ServiceProvider.GetRequiredService<TSystem>();

    public TSystem Create<TSystem>(Func<Resolver, TSystem> factory) => factory(gameContext.ServiceProvider.GetRequiredService);
}
