using DualBlade.Core.Scenes;
using DualBlade.Core.Worlds;
using Microsoft.Extensions.DependencyInjection;

namespace DualBlade.Core.Services;

internal sealed class SceneManager(IGameContext gameContext, IServiceProvider serviceProvider) : ISceneManager
{
    private readonly IWorld world = gameContext.World;
    private readonly List<IGameScene> _activeScenes = [];

    public T CreateScene<T>() where T : IGameScene =>
        serviceProvider.GetRequiredService<T>();

    public T AddSceneExclusively<T>() where T : IGameScene
    {
        var scene = CreateScene<T>();
        AddSceneExclusively(scene);
        return scene;
    }

    public void AddSceneExclusively(IGameScene gameScene)
    {
        _activeScenes.ForEach(s => s.Dispose());
        AddScene(gameScene);
    }

    public T AddScene<T>() where T : IGameScene
    {
        var scene = CreateScene<T>();
        AddScene(scene);
        return scene;
    }

    public void AddScene(IGameScene gameScene)
    {
        foreach (var system in gameScene.Systems)
        {
            world.AddSystem(system);
        }

        gameScene.Root.AddToWorld(world);
        _activeScenes.Add(gameScene);
    }
}
