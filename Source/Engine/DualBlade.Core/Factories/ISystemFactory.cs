using DualBlade.Core.Systems;
using static DualBlade.Core.Factories.SystemFactory;

namespace DualBlade.Core.Factories;

public interface ISystemFactory
{
    TSystem Create<TSystem>() where TSystem : ISystem;
    TSystem Create<TSystem>(Func<Resolver, TSystem> factory);
}