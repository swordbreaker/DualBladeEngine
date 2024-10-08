﻿using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using DualBlade.Core.Worlds;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework.Content;
using Color = Microsoft.Xna.Framework.Color;

namespace DualBlade.Core;

public abstract class BaseGame : Game
{
    protected Color BackgroundColor = Color.CornflowerBlue;
    protected IGameContext Context;
    protected IWorld World;
    protected IGameEngine GameEngine;
    protected ISceneManager SceneManager;
    protected IServiceProvider ServiceProvider;

    public BaseGame(IGameCreationContext creationContext, ContentManager? contentManager = null)
    {
        Content.RootDirectory = "Content";
        Context = creationContext.CreateContext(this, contentManager);
        World = Context.World;

        GameEngine = Context.GameEngine;
        ServiceProvider = creationContext.ServiceProvider;
        SceneManager = Context.SceneManager;
    }

    protected override void Initialize()
    {
        GameEngine.Initialize();

        foreach (var startupService in Context.ServiceProvider.GetRequiredService<IEnumerable<IStartupService>>())
        {
            startupService.OnStart(Context);
        }

        InitializeGlobalSystems();

        Context.World.Initialize();
        base.Initialize();
    }

    protected virtual void InitializeGlobalSystems()
    {
        World.AddSystem<InputSystem>();
    }

    protected override void Update(GameTime gameTime)
    {
        World.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(BackgroundColor);
        World.Draw(gameTime);
        base.Draw(gameTime);
    }
}
