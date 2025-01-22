using DualBlade.Core.Entities;
using DualBlade.Core.Systems;

namespace DualBlade.Core.Worlds;

/// <summary>
/// The world is the main container for all systems and entities.
/// The world will update and draw all systems and entities.
/// </summary>
public interface IWorld
{
    /// <summary>
    /// Get all systems in the world.
    /// </summary>
    IEnumerable<ISystem> Systems { get; }

    /// <summary>
    /// Get all entities in the world.
    /// </summary>
    IEnumerable<IEntity> Entities { get; }

    /// <summary>
    /// Initialize the world.
    /// </summary>
    void Initialize();

    /// <summary>
    ///  Update the world. This can be called multiple times per frame.
    /// </summary>
    /// <param name="gameTime"></param>
    void Update(GameTime gameTime);

    /// <summary>
    /// Draw the world. This will be called once per frame.
    /// </summary>
    /// <param name="gameTime"></param>
    void Draw(GameTime gameTime);

    #region Systems

    /// <summary>
    /// Add systems to the world. This will initialize the systems.
    /// If the system is already added, it will not be added again.
    /// </summary>
    /// <param name="systems"></param>
    /// <returns>All added systems. If the system was already in the world it will not be returned here.</returns>
    IEnumerable<ISystem> AddSystems(params ISystem[] systems);

    /// <summary>
    /// Add a system to the world. This will initialize the system.
    /// </summary>
    /// <typeparam name="TSystem"></typeparam>
    /// <returns>True when the system was added; false when the system was already present in the world.</returns>
    bool AddSystem<TSystem>() where TSystem : ISystem;

    /// <summary>
    /// Add a system to the world. This will initialize the system.
    /// </summary>
    /// <param name="system"></param>
    /// <returns>True when the system was added; false when the system was already present in the world.</returns>
    bool AddSystem(ISystem system);

    /// <summary>
    /// Destroy a system from the world.
    /// </summary>
    /// <param name="system"></param>
    void Destroy(ISystem system);

    /// <summary>
    /// Destroy systems from the world.
    /// </summary>
    /// <param name="systems"></param>
    void Destroy(IEnumerable<ISystem> systems);

    #endregion

    #region Entity

    /// <summary>
    /// Add entities to the world.
    /// </summary>
    /// <param name="entities"></param>
    void AddEntities(params IEntity[] entities);

    /// <summary>
    /// Add an entity to the world.s
    /// </summary>
    /// <param name="entity"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    TEntity AddEntity<TEntity>(TEntity entity) where TEntity : IEntity;

    /// <summary>
    /// Get an entity from the world.
    /// </summary>s
    /// <param name="id"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    EntityProxy<TEntity> GetEntityProxy<TEntity>(int id) where TEntity : IEntity;

    /// <summary>
    /// Get an entity reference from the world.
    /// </summary>
    /// <param name="id"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    EntityRef<TEntity> GetEntityRef<TEntity>(int id) where TEntity : IEntity;

    /// <summary>
    /// Update an entity in the world.
    /// </summary>
    /// <param name="entity"></param>
    void UpdateEntity(IEntity entity);

    /// <summary>
    /// Destroy an entity from the world.
    /// </summary>
    /// <param name="entity"></param>
    void Destroy(IEntity entity);

    #endregion
}