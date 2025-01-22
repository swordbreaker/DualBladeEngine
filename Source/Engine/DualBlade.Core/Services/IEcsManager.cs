using DualBlade.Core.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Worlds;

namespace DualBlade.Core.Services;

/// <summary>
/// Entity Component System Manager.
/// Used to manipulate entities and components in the world and get the hierachy of entities.
/// </summary>
public interface IEcsManager
{
    /// <summary>
    /// Get the entity of a component.
    /// </summary>
    /// <param name="component"></param>
    /// <returns></returns>
    EntityProxy<IEntity> GetEntity(IComponent component);

    /// <summary>
    /// Destroy an entity.
    /// </summary>
    /// <param name="entity"></param>
    void DestroyEntity(IEntity entity);

    /// <summary>
    /// Traverse the entity tree to the root.
    /// </summary>
    /// <param name="node">The entity to traverse.</param>
    /// <param name="action">Action to call on each traversed entity.</param>
    void TraverseToParent(IEntity node, Action<IEntity> action);

    /// <summary>
    /// Add a parent to an entity.
    /// This also removes the entity form the old parent.
    /// </summary>
    /// <param name="child"></param>
    /// <param name="parent"></param>
    void AddParent(IEntity child, IEntity parent);

    /// <summary>
    /// Add a child to an entity.
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="child"></param>
    void AddChild(IEntity parent, IEntity child);

    /// <summary>
    /// Gets a proxy to the parent entity.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public EntityProxy<IEntity> GetParent(IEntity entity);

    /// <summary>
    /// Gets the children of an entity.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public IEnumerable<EntityRef<IEntity>> GetChildren(IEntity entity);

    void UpdateComponent(IEntity entity, IComponent component);
    void UpdateEntity(IEntity entity);
}