using DualBlade.Core.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Worlds;

namespace DualBlade.Core.Services;

/// <summary>
/// Entity Component System Manager.
/// Used to manipulate entities and components in the world and get the hierarchy of entities.
/// </summary>
public interface IEcsManager
{
    /// <summary>
    /// Get the entity of a component.
    /// </summary>
    /// <param name="component">The component to get the entity for.</param>
    /// <returns>A proxy to the entity that owns the component.</returns>
    EntityProxy<IEntity> GetEntity(IComponent component);

    /// <summary>
    /// Destroy an entity.
    /// </summary>
    /// <param name="entity">The entity to destroy.</param>
    void DestroyEntity(IEntity entity);

    /// <summary>
    /// Traverse the entity tree to the root.
    /// </summary>
    /// <param name="node">The entity to traverse.</param>
    /// <param name="action">Action to call on each traversed entity.</param>
    void TraverseToParent(IEntity node, Action<IEntity> action);

    /// <summary>
    /// Add a parent to an entity.
    /// This also removes the entity from the old parent.
    /// </summary>
    /// <param name="child">The child entity.</param>
    /// <param name="parent">The new parent entity.</param>
    void AddParent(IEntity child, IEntity parent);

    /// <summary>
    /// Add a child to an entity.
    /// </summary>
    /// <param name="parent">The parent entity.</param>
    /// <param name="child">The child entity to add.</param>
    void AddChild(IEntity parent, IEntity child);

    /// <summary>
    /// Gets a proxy to the parent entity.
    /// </summary>
    /// <param name="entity">The entity to get the parent for.</param>
    /// <returns>A proxy to the parent entity.</returns>
    public EntityProxy<IEntity> GetParent(IEntity entity);

    /// <summary>
    /// Gets the children of an entity.
    /// </summary>
    /// <param name="entity">The entity to get the children for.</param>
    /// <returns>An enumerable collection of entity references representing the children.</returns>
    public IEnumerable<EntityRef<IEntity>> GetChildren(IEntity entity);

    /// <summary>
    /// Updates a component on an entity.
    /// </summary>
    /// <param name="entity">The entity to update the component on.</param>
    /// <param name="component">The component to update.</param>
    void UpdateComponent(IEntity entity, IComponent component);

    /// <summary>
    /// Updates an entity in the world.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    void UpdateEntity(IEntity entity);

    /// <summary>
    /// Gets a single entity of the specified type. Throws an exception if no entity or multiple entities are found.
    /// </summary>
    /// <typeparam name="T">The type of entity to find.</typeparam>
    /// <returns>The single entity of the specified type.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no entity or multiple entities of the specified type are found.</exception>
    public T SingelEntity<T>() where T : IEntity;

    /// <summary>
    /// Gets all entities of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of entities to find.</typeparam>
    /// <returns>An enumerable collection of entities of the specified type.</returns>
    public IEnumerable<T> GetEntities<T>() where T : IEntity;

    /// <summary>
    /// Gets a single component of the specified type. Throws an exception if no component or multiple components are found.
    /// </summary>
    /// <typeparam name="T">The type of component to find.</typeparam>
    /// <returns>The single component of the specified type.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no component or multiple components of the specified type are found.</exception>
    public T SignelComponent<T>() where T : IComponent;

    /// <summary>
    /// Gets all components of the specified type across all entities.
    /// </summary>
    /// <typeparam name="T">The type of components to find.</typeparam>
    /// <returns>An enumerable collection of components of the specified type.</returns>
    public IEnumerable<T> GetComponents<T>() where T : IComponent;
}