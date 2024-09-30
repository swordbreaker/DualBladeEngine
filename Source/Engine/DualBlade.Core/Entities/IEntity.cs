using DualBlade.Core.Collections;
using DualBlade.Core.Components;
using DualBlade.Core.Worlds;

namespace DualBlade.Core.Entities;

public interface IEntity
{
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

    void AddParentEntity(IEntity parent)
    {

    }

    bool HasComponent<TComponent>() where TComponent : IComponent;
    ComponentProxy<TComponent> Component<TComponent>() where TComponent : IComponent;

    bool TryGetComponent<TComponent>(out ComponentProxy<TComponent> componentProxy) where TComponent : IComponent;
    ComponentProxy<TComponent> AddComponent<TComponent>(TComponent component) where TComponent : IComponent;
    void RemoveComponent<TComponent>() where TComponent : IComponent;
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

    void Init(int id);
}