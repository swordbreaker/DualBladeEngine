using MonoGameEngine.Engine.Worlds;

namespace MonoGameEngine.Engine.Systems;

public interface ISystemWithWorld : ISystem
{
    IWorld World { get; init; }
}
