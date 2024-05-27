using MonoGameEngine.Engine.Entities;
using MonoGameEngine.Engine.Worlds;
using System.Data.SqlTypes;

namespace MonoGameEngine.Engine.Systems;

public class EntitySystem<TEntity> : IEntitySystem where TEntity : IEntity
{
    private IGameEngine? _gameEngine;

    public required IWorld World { get; init; }

    protected virtual void Initialize(TEntity entity, IGameEngine gameEngine) { }
    protected virtual void Update(TEntity entity, GameTime gameTime, IGameEngine gameEngine) { }
    protected virtual void Draw(TEntity entity, GameTime gameTime, IGameEngine gameEngine) { }

    public void Initialize(IGameEngine gameEngine)
    {
        this._gameEngine = gameEngine;

        foreach (var entity in World.GetEntities<TEntity>())
        {
            Initialize(entity, gameEngine);
        }

        World.EntityAdded += Init;
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

    public void Dispose() =>
        World.EntityAdded -= Init;

    private void Init(IEntity e)
    {
        if (e is TEntity entity && _gameEngine is not null)
        {
            Initialize(entity, _gameEngine);
        }
    }
}
