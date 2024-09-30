using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;

namespace DualBlade.Core.Scenes;
public interface IGameScene : IDisposable
{
    EntityBuilder Root { get; }

    IEnumerable<ISystem> Systems { get; }
}
