using DualBlade.Core.Services;

namespace DualBlade.Core.Factories;

public interface IGameEngineFactory
{
    IGameEngine Create(Game game);
}