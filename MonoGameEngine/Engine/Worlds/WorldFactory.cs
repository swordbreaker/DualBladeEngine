using MonoGameEngine.Engine.Systems;

namespace MonoGameEngine.Engine.Worlds;

public class WorldFactory(ISystemFactory _systemFactory, IJobQueue jobQueue) : IWorldFactory
{
    public IWorld Create(IGameEngine engine) =>
        new World(engine, _systemFactory, jobQueue);
}
