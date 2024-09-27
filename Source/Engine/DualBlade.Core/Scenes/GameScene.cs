﻿using DualBlade.Core.Entities;
using DualBlade.Core.Factories;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using Microsoft.Extensions.DependencyInjection;

namespace DualBlade.Core.Scenes;

public abstract class GameScene : IGameScene
{
    protected readonly IGameContext GameContext;
    protected readonly ISystemFactory SystemFactory;

    public GameScene(IGameContext context)
    {
        Systems = SetupSystems();
        GameContext = context;
        SystemFactory = context.ServiceProvider.GetRequiredService<ISystemFactory>();

        Root = new RootEntity();
        //{
        //    Children = SetupEntities(),
        //};
    }

    public virtual IEntity Root { get; }

    public IEnumerable<ISystem> Systems { get; }

    protected abstract IEnumerable<INodeEntity> SetupEntities();
    public abstract IEnumerable<ISystem> SetupSystems();

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        GameContext.World.Destroy(Root);
        GameContext.World.Destroy(Systems);
    }

    protected TSystem CreateSystem<TSystem>() where TSystem : ISystem =>
        SystemFactory.Create<TSystem>();

    protected T Resolve<T>() where T : notnull =>
        GameContext.ServiceProvider.GetRequiredService<T>();
}