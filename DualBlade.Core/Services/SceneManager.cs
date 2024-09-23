using DualBlade.Core.Scenes;
using DualBlade.Core.Worlds;
using Microsoft.Extensions.DependencyInjection;

namespace DualBlade.Core.Services;

internal class SceneManager(IGameContext gameContext, IServiceProvider serviceProvider) : ISceneManager
{
    private readonly IWorld world = gameContext.World;
    private readonly List<IGameScene> _activeScenes = [];
    protected IReadOnlyList<IGameScene> ActiveScenes => _activeScenes;

    public T CreateScene<T>(params object[] additionalParameters) where T : IGameScene =>
        (T)ActivatorUtilities.CreateInstance(serviceProvider, typeof(T), additionalParameters.Prepend(gameContext).ToArray());

    public T AddSceneExclusively<T>(params object[] additionalParameters) where T : IGameScene
    {
        var scene = CreateScene<T>(additionalParameters);
        AddSceneExclusively(scene);
        return scene;
    }

    public void AddSceneExclusively(IGameScene gameScene)
    {
        _activeScenes.ForEach(s => s.Dispose());
        AddScene(gameScene);
    }

    public T AddScene<T>(params object[] additionalParameters) where T : IGameScene
    {
        var scene = CreateScene<T>(additionalParameters);
        AddScene(scene);
        return scene;
    }

    public void AddScene(IGameScene gameScene)
    {
        world.AddEntity(gameScene.Root);

        foreach (var system in gameScene.Systems)
        {
            world.AddSystem(system);
        }

        _activeScenes.Add(gameScene);
    }
}
