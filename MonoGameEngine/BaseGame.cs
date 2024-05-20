using MonoGameEngine.Engine.Entities;
using MonoGameEngine.Engine.Scenes;
using MonoGameEngine.Engine.Systems;
using MonoGameEngine.Engine.Worlds;
using Myra;
using Myra.Graphics2D.UI;
using System.Collections.Generic;
using Color = Microsoft.Xna.Framework.Color;

namespace MonoGameEngine;

public abstract class BaseGame : Game
{
    private readonly List<IGameScene> _activeScenes = [];

    protected IGameEngine GameEngine;
    protected IWorld GameWorld;
    protected Desktop? Desktop;
    protected Color BackgroundColor = Color.CornflowerBlue;
    protected IReadOnlyList<IGameScene> ActiveScenes => _activeScenes;

    public BaseGame(IWorldFactory worldFactory, IGameEngineFactory gameEngineFactory)
    {
        GameEngine = gameEngineFactory.Create(this);
        GameWorld = worldFactory.Create(GameEngine);
        Content.RootDirectory = "Content";
    }

    protected override void Initialize()
    {
        GameEngine.Initialize();
        InitializeGlobalSystems();
        MyraEnvironment.Game = this;
        Desktop = new Desktop();

        GameWorld.Initialize();
        base.Initialize();
    }

    protected virtual void InitializeGlobalSystems()
    {
        GameWorld.AddSystem<RenderSystem>();
        GameWorld.AddSystem<KinematicSystem>();
        GameWorld.AddSystems(new InputSystem());
    }

    protected override void Update(GameTime gameTime)
    {
        GameWorld.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(BackgroundColor);
        GameWorld.Draw(gameTime);

        Desktop!.Render();
        base.Draw(gameTime);
    }

    protected void AddEntity<TEntity>(TEntity entity) where TEntity : Entity
    {
        GameWorld.AddEntity(entity);
    }

    protected void AddSceneExclusively(IGameScene gameScene)
    {
        _activeScenes.ForEach(s => s.Dispose());
        AddScene(gameScene);
    }

    protected void AddScene(IGameScene gameScene)
    {
        foreach (var system in gameScene.Systems)
        {
            GameWorld.AddSystem(system);
        }

        GameWorld.AddEntity(gameScene.Root);
        _activeScenes.Add(gameScene);
    }
}
