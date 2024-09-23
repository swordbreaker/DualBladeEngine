using DualBlade.Core.Scenes;

namespace DualBlade.Core.Services;

public interface ISceneManager
{
    T AddScene<T>() where T : IGameScene;
    void AddScene(IGameScene gameScene);
    T AddSceneExclusively<T>() where T : IGameScene;
    void AddSceneExclusively(IGameScene gameScene);
    T CreateScene<T>() where T : IGameScene;
}
