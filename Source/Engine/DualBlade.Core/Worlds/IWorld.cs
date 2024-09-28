using DualBlade.Core.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Systems;

namespace DualBlade.Core.Worlds;

public interface IWorld
{
    IEnumerable<IComponent> Components { get; }
    IEnumerable<ISystem> Systems { get; }
    IEnumerable<IEntity> Entities { get; }

    void Initialize();
    void Update(GameTime gameTime);
    void Draw(GameTime gameTime);

    #region Systems
    void AddSystems(params ISystem[] systems);
    void AddSystem<TSystem>() where TSystem : ISystem;
    void AddSystem(ISystem system);
    void Destroy(ISystem system);
    void Destroy(IEnumerable<ISystem> systems);
    #endregion

    #region Entity
    void AddEntities(params IEntity[] entities);
    void AddEntity<TEntity>(TEntity entity) where TEntity : IEntity;

    EntityProxy<TEntity> GetEntityProxy<TEntity>(int id) where TEntity : IEntity;

    void UpdateIEntity(IEntity entity);

    void Destroy(IEntity entity);
    #endregion

    #region Components
    internal ComponentRef<TComponent> AddComponent<TComponent>(TComponent component, IEntity entity) where TComponent : IComponent;

    internal TComponent GetComponentCopy<TComponent>(int id) where TComponent : IComponent;
    internal ComponentProxy<TComponent> GetComponentProxy<TComponent>(int id) where TComponent : IComponent;
    internal ComponentRef<TComponent> GetComponent<TComponent>(int id) where TComponent : IComponent;

    void UpdateComponent(IComponent component);

    internal void Destroy(IComponent component);
    #endregion
}
