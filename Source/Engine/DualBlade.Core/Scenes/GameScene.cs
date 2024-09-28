using DualBlade.Core.Entities;
using DualBlade.Core.Factories;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using Microsoft.Extensions.DependencyInjection;

namespace DualBlade.Core.Scenes;

public abstract class GameScene : IGameScene
{
    protected readonly IGameContext GameContext;
    protected readonly ISystemFactory SystemFactory;
    protected readonly IEcsManager EcsManager;

    public GameScene(IGameContext context)
    {
        Systems = SetupSystems();
        GameContext = context;
        SystemFactory = context.ServiceProvider.GetRequiredService<ISystemFactory>();
        EcsManager = context.EcsManager;

        Root = new RootEntity(SetupEntities());
        //{
        //    Children = SetupEntities(),
        //};
    }

    public virtual INodeEntity Root { get; }

    public IEnumerable<ISystem> Systems { get; }

    protected abstract IEnumerable<IEntity> SetupEntities();
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