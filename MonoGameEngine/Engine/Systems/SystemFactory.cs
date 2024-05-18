using MonoGameEngine.Engine.Worlds;

namespace MonoGameEngine.Engine.Systems;

public class SystemFactory : ISystemFactory
{
    public TSystem Create<TSystem>(IWorld world) where TSystem : ISystemWithWorld, new() =>
        new() { World = world };
}
