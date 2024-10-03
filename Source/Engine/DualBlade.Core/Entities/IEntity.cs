using DualBlade.Core.Collections;
using DualBlade.Core.Components;
using DualBlade.Core.Worlds;

namespace DualBlade.Core.Entities;

public interface IEntity
{
    public delegate void UpdateComponentDelegate<TComponent>(ref TComponent component) where TComponent : IComponent;

    /// <summary>
    /// Get the id of the entity. This will be set on <see cref="IWorld.AddEntities(IEntity[])"/>
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Get the ids of the child entities.
    /// </summary>
    public GrowableMemory<int> Children { get; set; }

    /// <summary>
    /// Get the id of the parent entity.
    /// -1 for no parent.
    /// </summary>
    public int Parent { get; set; }

    /// <summary>
    /// Check if the entity has a component.
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    /// <returns>True when the component exists; else false.</returns>
    bool HasComponent<TComponent>() where TComponent : IComponent;

    /// <summary>
    /// Get a component from the entity. This will throw an exception if the component does not exist.
    /// </summary>
    /// <exception cref="InvalidOperationException">When the component does not exist.</exception>
    /// <returns>An proxy to the component <see cref="ComponentProxy{T}"/> </returns>
    ComponentProxy<TComponent> Component<TComponent>() where TComponent : IComponent;

    /// <summary>
    /// Try get the component from the entity.
    /// </summary>
    /// <param name="componentProxy">A proxy to the component <see cref="ComponentProxy{T}"/></param>
    /// <returns>True when the component exists; else false.</returns>
    bool TryGetComponent<TComponent>(out ComponentProxy<TComponent> componentProxy) where TComponent : IComponent;

    /// <summary>
    /// Add a component to the entity.
    /// </summary>
    /// <returns>A proxy to the component <see cref="ComponentProxy{T}"/>.</returns>
    ComponentProxy<TComponent> AddComponent<TComponent>(TComponent component) where TComponent : IComponent;

    /// <summary>
    /// Remove a component from the entity.
    /// </summary>
    /// <typeparam name="TComponent">The component type.</typeparam>
    void RemoveComponent<TComponent>() where TComponent : IComponent;

    /// <summary>
    /// Update a component on the entity.
    /// </summary>
    void UpdateComponent<TComponent>(TComponent component) where TComponent : IComponent;

    /// <summary>
    /// Internal components collection, do not use this directly.
    /// </summary>
    GrowableMemory<IComponent> InternalComponents { get; }

    /// <summary>
    /// Gets all the components that the entity has.
    /// Always sorted by component type.
    /// </summary>
    IEnumerable<IComponent> Components { get; }

    /// <summary>
    /// Gets all the types of the components that the entity has.
    /// Always sorted.
    /// </summary>
    Memory<Type> ComponentTypes { get; }

    /// <summary>
    /// Will be called by the <see cref="IWorld"/>. Do not call this directly.
    /// </summary>
    void Init(int id);
}