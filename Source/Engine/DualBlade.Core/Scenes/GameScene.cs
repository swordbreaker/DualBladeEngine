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
    private IEntity? rootEntity;

    public GameScene(IGameContext context)
    {
        Systems = SetupSystems();
        GameContext = context;
        SystemFactory = context.ServiceProvider.GetRequiredService<ISystemFactory>();
        EcsManager = context.EcsManager;

        Root = new EntityBuilder(new RootEntity(), e => rootEntity = e)
            .AddChildren(SetupEntities());
    }

    public virtual EntityBuilder Root { get; }

    public IEnumerable<ISystem> Systems { get; }

    protected abstract IEnumerable<EntityBuilder> SetupEntities();
    public abstract IEnumerable<ISystem> SetupSystems();

    protected virtual void OnDispose()
    {
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        OnDispose();

        if (rootEntity is not null)
        {
            GameContext.World.Destroy(rootEntity);

            // TODO when system was already in the world when the scene was added, it should not be destroyed
            GameContext.World.Destroy(Systems);
        }
    }

    protected TSystem CreateSystem<TSystem>() where TSystem : ISystem =>
        SystemFactory.Create<TSystem>();

    protected static EntityBuilder<TEntity> CreateEntity<TEntity>() where TEntity : struct, IEntity =>
        new(new TEntity());

    protected static EntityBuilder<TEntity> CreateEntity<TEntity>(TEntity entity) where TEntity : struct, IEntity =>
        new(entity);

    protected T Resolve<T>() where T : notnull =>
        GameContext.ServiceProvider.GetRequiredService<T>();
}