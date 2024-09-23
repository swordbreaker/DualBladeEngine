using DualBlade.Core.Entities;
using DualBlade.Core.Systems;

namespace DualBlade.Core.Scenes;
public interface IGameScene : IDisposable
{
    IEntity Root { get; }

    IEnumerable<ISystem> Systems { get; }
}
