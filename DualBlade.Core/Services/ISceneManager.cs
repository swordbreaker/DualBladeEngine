using DualBlade.Core.Scenes;

namespace DualBlade.Core.Services;

public interface ISceneManager
{
    T AddScene<T>(params object[] additionalParameters) where T : IGameScene;
    void AddScene(IGameScene gameScene);
    T AddSceneExclusively<T>(params object[] additionalParameters) where T : IGameScene;
    void AddSceneExclusively(IGameScene gameScene);
    T CreateScene<T>(params object[] additionalParameters) where T : IGameScene;
}
