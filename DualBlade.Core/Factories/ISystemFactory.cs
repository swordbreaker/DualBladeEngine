using DualBlade.Core.Systems;

namespace DualBlade.Core.Factories;

public interface ISystemFactory
{
    TSystem Create<TSystem>(object[] additionalParameters) where TSystem : ISystem;
}