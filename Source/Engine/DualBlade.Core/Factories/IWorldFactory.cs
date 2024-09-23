using DualBlade.Core.Services;
using DualBlade.Core.Worlds;

namespace DualBlade.Core.Factories;

public interface IWorldFactory
{
    IWorld Create(IGameEngine engine);
}