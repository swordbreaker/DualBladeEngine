
using DualBlade.Core.Worlds;

namespace DualBlade.Core.Services;

public interface IGameContext
{
    IServiceProvider ServiceProvider { get; }
    IGameEngine GameEngine { get; }
    BaseGame Game { get; }
    IWorld World { get; }
    ISceneManager SceneManager { get; }
}