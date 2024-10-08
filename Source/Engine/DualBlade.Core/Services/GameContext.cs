﻿using AutomaticInterface;
using DualBlade.Core.Worlds;

namespace DualBlade.Core.Services;

internal interface IGameContextInitializer
{
    void Init(IGameEngine gameEngine, BaseGame baseGame, IWorld world);
}

public sealed class GameContext(IServiceProvider _serviceProvider, ISceneManagerFactory sceneManagerFactory) : IGameContext, IGameContextInitializer
{
    private bool isInitialized = false;
    private IGameEngine? _gameEngine;
    private BaseGame? _game;
    private IWorld? _world;
    private ISceneManager? _sceneManager;
    private IEcsManager _ecsManager;

    public void Init(IGameEngine gameEngine, BaseGame baseGame, IWorld world)
    {
        _gameEngine = gameEngine;
        _game = baseGame;
        _world = world;
        isInitialized = true;
        _sceneManager = sceneManagerFactory.CreateSceneManager(this);
        _ecsManager = new EcsManager(world);
    }

    public IServiceProvider ServiceProvider => _serviceProvider;

    public IGameEngine GameEngine
    {
        get => GetValueSafe(_gameEngine);
        private set => _gameEngine = value;
    }

    public BaseGame Game
    {
        get => GetValueSafe(_game);
        private set => _game = value;
    }

    public IWorld World
    {
        get => GetValueSafe(_world);
        private set => _world = value;
    }

    public ISceneManager SceneManager
    {
        get => GetValueSafe(_sceneManager);
        private set => _sceneManager = value;
    }

    public IEcsManager EcsManager
    {
        get => GetValueSafe(_ecsManager);
        private set => _ecsManager = value;
    }

    private T GetValueSafe<T>(T? vale) => GetValueSafe(() => vale);

    private T GetValueSafe<T>(Func<T?> func)
    {
        if (!isInitialized || func() is not T value)
        {
            throw new InvalidOperationException("GameContext not initialized, have you implemented the BaseGame class and called it at the start?");
        }

        return value;
    }

}
