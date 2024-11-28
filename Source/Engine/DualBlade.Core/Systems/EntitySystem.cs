using DualBlade.Core.Entities;
using DualBlade.Core.Services;

namespace DualBlade.Core.Systems;

public class EntitySystem<TEntity>(IGameContext gameContext)
    : BaseSystem(gameContext), IEntitySystem where TEntity : IEntity
{
    Type IEntitySystem.EntityType { get; } = typeof(TEntity);

    protected virtual void OnAdded(ref TEntity entity)
    {
    }

    protected virtual void Update(ref TEntity entity, GameTime gameTime)
    {
    }

    public virtual void OnDestroy(TEntity entity)
    {
    }

    protected virtual void Draw(TEntity entity, GameTime gameTime)
    {
    }

    IEntity IEntitySystem.Update(IEntity component, GameTime gameTime)
    {
        var ent = (TEntity)component;

        this.Update(ref ent, gameTime);
        return ent;
    }

    void IEntitySystem.LateUpdate(GameTime gameTime)
    {
        this.LateUpdate(gameTime);
    }

    void IEntitySystem.Draw(IEntity entity, GameTime gameTime)
    {
        this.Draw((TEntity)entity, gameTime);
    }

    public override void Initialize()
    {
    }

    public override void Update(GameTime gameTime)
    {
    }

    public virtual void LateUpdate(GameTime gameTime)
    {
    }

    public override void Draw(GameTime gameTime)
    {
    }

    public virtual void LateDraw(GameTime gameTime)
    {
    }

    public override void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    IEntity IEntitySystem.OnAdded(IEntity entity)
    {
        var e = (TEntity)entity;
        OnAdded(ref e);
        return e;
    }

    void IEntitySystem.OnDestroy(IEntity entity) =>
        OnDestroy((TEntity)entity);
}