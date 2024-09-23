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
    protected readonly IEntityFactory EntityFactory;

    public GameScene(IGameContext context)
    {
        Systems = SetupSystems();
        GameContext = context;
        SystemFactory = context.ServiceProvider.GetRequiredService<ISystemFactory>();
        EntityFactory = context.ServiceProvider.GetRequiredService<IEntityFactory>();

        Root = new RootEntity
        {
            Children = SetupEntities(),
        };
    }

    public IEntity Root { get; }

    public IEnumerable<ISystem> Systems { get; }

    protected abstract IEnumerable<IEntity> SetupEntities();
    public abstract IEnumerable<ISystem> SetupSystems();

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        GameContext.World.Destroy(Root);
        GameContext.World.Destroy(Systems);
    }

    protected TEntity CreateEntity<TEntity>() where TEntity : IEntity =>
        EntityFactory.CreateEntity<TEntity>();

    protected TSystem CreateSystem<TSystem>(params object[] additionalParameters) where TSystem : ISystemWithContext =>
        SystemFactory.Create<TSystem>(additionalParameters);
}