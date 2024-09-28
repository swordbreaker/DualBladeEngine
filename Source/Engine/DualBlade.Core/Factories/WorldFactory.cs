using DualBlade.Core.Services;
using DualBlade.Core.Worlds;

namespace DualBlade.Core.Factories;

public sealed class WorldFactory(ISystemFactory _systemFactory, IJobQueue jobQueue) : IWorldFactory
{
    public IWorld Create(IGameEngine engine) =>
        new World(_systemFactory, jobQueue);
}
