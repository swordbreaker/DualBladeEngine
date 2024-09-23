using DualBlade.Core.Services;
using DualBlade.Core.Worlds;

namespace DualBlade.Core.Systems;

public interface ISystemWithContext : ISystem
{
    IGameContext GameContext { get; }
    IWorld World { get; }
}
