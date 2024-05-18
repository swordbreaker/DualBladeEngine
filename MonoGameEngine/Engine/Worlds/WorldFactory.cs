using MonoGameEngine.Engine.Systems;

namespace MonoGameEngine.Engine.Worlds;

public class WorldFactory(ISystemFactory _systemFactory) : IWorldFactory
{
    public IWorld Create(IGameEngine engine) =>
        new World(engine, _systemFactory);
}
