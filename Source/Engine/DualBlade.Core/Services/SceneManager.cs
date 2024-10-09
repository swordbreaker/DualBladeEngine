using DualBlade.Core.Entities;
using DualBlade.Core.Scenes;
using DualBlade.Core.Systems;
using DualBlade.Core.Worlds;
using Microsoft.Extensions.DependencyInjection;

namespace DualBlade.Core.Services;

internal record SceneData(IGameScene Scene, IEntity? Root, IReadOnlyList<ISystem> systems);

internal sealed class SceneManager(IGameContext gameContext, IServiceProvider serviceProvider) : ISceneManager
{
    private readonly IWorld world = gameContext.World;
    private readonly Dictionary<IGameScene, SceneData> _activeScenes = [];

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
        foreach (var scene in _activeScenes.Values)
        {
            RemoveScene(scene);
        }

        _activeScenes.Clear();
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
        var addedSystems = new List<ISystem>();

        foreach (var system in gameScene.Systems)
        {
            if (world.AddSystem(system))
            {
                addedSystems.Add(system);
            }
        }

        var rootEntity = gameScene.Root.AddToWorld(world);

        _activeScenes.Add(gameScene, new(gameScene, rootEntity, addedSystems));
    }

    public void RemoveScene(IGameScene gameScene)
    {
        if (!_activeScenes.TryGetValue(gameScene, out var sceneData))
        {
            throw new InvalidOperationException("Scene not found");
        }

        RemoveScene(sceneData);

        _activeScenes.Remove(gameScene);
    }

    private void RemoveScene(SceneData sceneData)
    {
        var (scene, rootEntity, systems) = sceneData;
        if (rootEntity is not null)
        {
            world.Destroy(rootEntity);
        }
        world.Destroy(systems);
        scene.Dispose();
    }
}
