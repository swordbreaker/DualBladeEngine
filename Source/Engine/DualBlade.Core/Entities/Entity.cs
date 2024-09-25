using DualBlade.Core.Components;
using DualBlade.Core.Worlds;

namespace DualBlade.Core.Entities;

public class Entity : IEntity
{
    private readonly Dictionary<Type, IComponent> _components = [];
    private IWorld? _world;

    public IEnumerable<IComponent> Components => _components.Values;

    public void Initialize(IWorld world) => _world = world;

    public TComponent AddComponent<TComponent>() where TComponent : IComponent, new()
    {
        var component = new TComponent();
        AddComponent(component);
        return component;
    }

    public void AddComponent<TComponent>(TComponent component) where TComponent : IComponent
    {
        if (_world is not null)
        {
            _world.AddComponent(this, component);
        }
        else
        {
            _components.Add(typeof(TComponent), component);
        }
    }

    public void RemoveComponent<TComponent>() where TComponent : IComponent =>
        RemoveComponent(typeof(TComponent));

    public void RemoveComponent(Type type) =>
        _components.Remove(type);
}