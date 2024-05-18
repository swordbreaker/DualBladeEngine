using MonoGameEngine.Engine.Worlds;

namespace MonoGameEngine.Engine.Systems;

public interface ISystemFactory
{
    TSystem Create<TSystem>(IWorld world) where TSystem : ISystemWithWorld, new();
}