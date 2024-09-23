using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using DualBlade.Core.Worlds;

namespace DualBlade.Core.Factories;

public class WorldFactory(ISystemFactory _systemFactory, IJobQueue jobQueue) : IWorldFactory
{
    public IWorld Create(IGameEngine engine) =>
        new World(engine, _systemFactory, jobQueue);
}
