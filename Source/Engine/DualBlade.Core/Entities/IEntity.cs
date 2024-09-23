using IComponent = DualBlade.Core.Components.IComponent;

namespace DualBlade.Core.Entities;

public interface IEntity
{
    IEnumerable<IComponent> Components { get; }
    TComponent AddComponent<TComponent>() where TComponent : IComponent, new();
    void AddComponent<TComponent>(TComponent component) where TComponent : IComponent;
    void RemoveComponent<TComponent>() where TComponent : IComponent;
    void RemoveComponent(Type type);
}
