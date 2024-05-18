using Microsoft.Xna.Framework;
using MonoGameEngine.Engine.Entities;
using MonoGameEngine.Engine.Worlds;

namespace MonoGameEngine.Engine.Systems;

public class EntitySystem<TEntity> : IEntitySystem where TEntity : IEntity
{
    public required IWorld World { get; init; }

    protected virtual void Initialize(TEntity entity, IGameEngine gameEngine) { }
    protected virtual void Update(TEntity entity, GameTime gameTime, IGameEngine gameEngine) { }
    protected virtual void Draw(TEntity entity, GameTime gameTime, IGameEngine gameEngine) { }

    public void Initialize(IGameEngine gameEngine)
    {
        foreach (var entity in World.GetEntities<TEntity>())
        {
            Initialize(entity, gameEngine);
        }
    }

    public void Update(GameTime gameTime, IGameEngine gameEngine)
    {
        foreach (var entity in World.GetEntities<TEntity>())
        {
            Update(entity, gameTime, gameEngine);
        }
    }

    public void Draw(GameTime gameTime, IGameEngine gameEngine)
    {
        foreach (var entity in World.GetEntities<TEntity>())
        {
            Draw(entity, gameTime, gameEngine);
        }
    }
}
