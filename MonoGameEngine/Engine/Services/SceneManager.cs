using System.Collections.Generic;
using AutomaticInterface;
using MonoGameEngine.Engine.Scenes;
using MonoGameEngine.Engine.Worlds;

namespace MonoGameEngine.Engine.Services;

[GenerateAutomaticInterface]
public class SceneManager(IWorld gameWorld) : ISceneManager
{
    private readonly List<IGameScene> _activeScenes = [];
    protected IReadOnlyList<IGameScene> ActiveScenes => _activeScenes;

    public void AddSceneExclusively(IGameScene gameScene)
    {
        _activeScenes.ForEach(s => s.Dispose());
        AddScene(gameScene);
    }

    public void AddScene(IGameScene gameScene)
    {
        gameWorld.AddEntity(gameScene.Root);

        foreach (var system in gameScene.Systems)
        {
            gameWorld.AddSystem(system);
        }

        _activeScenes.Add(gameScene);
    }
}
