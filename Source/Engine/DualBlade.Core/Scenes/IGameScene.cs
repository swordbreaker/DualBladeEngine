using DualBlade.Core.Entities;
using DualBlade.Core.Systems;

namespace DualBlade.Core.Scenes;
public interface IGameScene : IDisposable
{
    INodeEntity Root { get; }

    IEnumerable<ISystem> Systems { get; }
}
