using DualBlade.Core.Entities;
using DualBlade.Core.Systems;

namespace DualBlade.Core.Worlds;

public interface IWorld
{
    IEnumerable<ISystem> Systems { get; }
    IEnumerable<IEntity> Entities { get; }

    void Initialize();
    void Update(GameTime gameTime);
    void Draw(GameTime gameTime);

    #region Systems
    IEnumerable<ISystem> AddSystems(params ISystem[] systems);
    bool AddSystem<TSystem>() where TSystem : ISystem;
    bool AddSystem(ISystem system);
    void Destroy(ISystem system);
    void Destroy(IEnumerable<ISystem> systems);
    #endregion

    #region Entity
    void AddEntities(params IEntity[] entities);
    TEntity AddEntity<TEntity>(TEntity entity) where TEntity : IEntity;

    EntityProxy<TEntity> GetEntityProxy<TEntity>(int id) where TEntity : IEntity;
    EntityRef<TEntity> GetEntityRef<TEntity>(int id) where TEntity : IEntity;

    void UpdateEntity(IEntity entity);

    void Destroy(IEntity entity);
    #endregion
}
