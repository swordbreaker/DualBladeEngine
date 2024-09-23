using DualBlade.Core.Entities;
using DualBlade.Core.Services;

namespace DualBlade.Core.Systems;

public class EntitySystem<TEntity>(IGameContext gameContext) : BaseSystem(gameContext), IEntitySystem where TEntity : IEntity
{
    protected virtual void Initialize(TEntity entity) { }
    protected virtual void Update(TEntity entity, GameTime gameTime) { }
    protected virtual void Draw(TEntity entity, GameTime gameTime) { }

    public override void Initialize()
    {
        foreach (var entity in World.GetEntities<TEntity>())
        {
            Initialize(entity);
        }

        World.EntityAdded += Init;
    }

    public override void Update(GameTime gameTime)
    {
        foreach (var entity in World.GetEntities<TEntity>())
        {
            Update(entity, gameTime);
        }
    }

    public override void Draw(GameTime gameTime)
    {
        foreach (var entity in World.GetEntities<TEntity>())
        {
            Draw(entity, gameTime);
        }
    }

    public override void Dispose()
    {
        GC.SuppressFinalize(this);
        World.EntityAdded -= Init;
    }

    private void Init(IEntity e)
    {
        if (e is TEntity entity)
        {
            Initialize(entity);
        }
    }
}
