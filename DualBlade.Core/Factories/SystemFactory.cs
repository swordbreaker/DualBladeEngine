using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using Microsoft.Extensions.DependencyInjection;

namespace DualBlade.Core.Factories;

internal class SystemFactory(IGameContext gameContext) : ISystemFactory
{
    public TSystem Create<TSystem>(object[] additionalParameters) where TSystem : ISystem
    {
        if (typeof(TSystem).IsAssignableTo(typeof(ISystemWithContext)))
        {
            var parameters = additionalParameters.Prepend(gameContext).ToArray();
            return (TSystem)ActivatorUtilities.CreateInstance(gameContext.ServiceProvider, typeof(TSystem), parameters);
        }

        return (TSystem)ActivatorUtilities.CreateInstance(gameContext.ServiceProvider, typeof(TSystem), additionalParameters);
    }
}
