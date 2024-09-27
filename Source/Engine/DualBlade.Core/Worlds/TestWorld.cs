using DualBlade.Core.Collections;
using System.Runtime.CompilerServices;

namespace DualBlade.Core.Worlds;

public abstract class TestSystem
{
    internal abstract Type CompType { get; }
    public virtual void Update() { }
    internal abstract ITestComponent Update(ITestComponent component);
}

public abstract class TestSystem<TComponent> : TestSystem where TComponent : ITestComponent
{
    internal override Type CompType => typeof(TComponent);

    internal override ITestComponent Update(ITestComponent component)
    {
        var comp = (TComponent)component;

        this.Update(ref comp);
        return comp;
    }

    protected abstract void Update(ref TComponent component);
}

public interface ITestComponent
{
    int Id { get; set; }
    int EntityId { get; set; }
}


public interface ITestNodeComponent : ITestComponent
{
    ComponentRef<ITestComponent>? Parent { get; }
    GrowableMemory<ComponentRef<ITestComponent>> Children { get; }
}

public struct TestNodeComponent : ITestComponent, ITestNodeComponent
{
    public ComponentRef<ITestComponent>? Parent { get; }

    public GrowableMemory<ComponentRef<ITestComponent>> Children { get; } = new(5);
    public int Id { get; set; }
    public int EntityId { get; set; }

    public TestNodeComponent() { }
}

public interface ITestEntity
{
    public int Id { get; }
}

public interface ITestNodeEntity
{
    ComponentRef<ITestNodeComponent> NodeComponent { get; init; }
}

public interface IInternalEntity
{
    ComponentRef<TComponent> Component<TComponent>() where TComponent : ITestComponent;

    Span<ITestComponent> InitialComponent { get; }
    void Init(
        TestWorld.AddComponentDelegate addComponent,
        int id);
}

public struct TestEntity : ITestEntity, IInternalEntity
{
    public int Id { get; private set; }
    private TestWorld.AddComponentDelegate? _addComponent;
    private GrowableMemory<(Type, int)> componentsIds = new(100);

    private GrowableMemory<ITestComponent> _initialComponents = new(100);
    private GrowableMemory<ComponentRef<ITestComponent>> _componentRefs = new(100);

    Span<ITestComponent> IInternalEntity.InitialComponent => _initialComponents.ToSpan();

    public TestEntity()
    {
    }

    void IInternalEntity.Init(TestWorld.AddComponentDelegate addComponent, int id)
    {
        _addComponent = addComponent;
        Id = id;
        _componentRefs.Clear();
    }

    public readonly void AddComponent(ITestComponent component)
    {
        if (componentsIds.Contains(x => x.Item1 == component.GetType()))
        {
            throw new InvalidOperationException("Component already added");
        }

        if (_addComponent is not null)
        {
            _componentRefs.Add(_addComponent(component));
        }
        else
        {
            _initialComponents.Add(component);
        }
    }

    public readonly ComponentRef<TComponent> Component<TComponent>() where TComponent : ITestComponent
    {
        var cRef = _componentRefs.Find(x => x.GetCopy().GetType() == typeof(TComponent));
        return cRef.As<TComponent>();
    }
}

public class TestWorld
{
    public delegate ComponentRef<ITestComponent> AddComponentDelegate(ITestComponent component);
    public delegate ITestComponent GetCopyDelegate(int id);
    public delegate ComponentProxy<ITestComponent> GetProxyDelegate<TComponent>();

    public SparseCollection<ITestComponent> _components = new(10000);
    public GrowableMemory<TestSystem> _systems = new(100);
    public SparseCollection<ITestEntity> entities = new(100);

    public ComponentRef<TComponent> AddComponent<TComponent>(TComponent component) where TComponent : ITestComponent
    {
        var id = _components.NextFreeIndex();
        component.Id = id;
        _components.Add(component);
        return new ComponentRef<TComponent>(this, id);
    }

    public void AddEntity(ITestEntity entity)
    {
        var index = entities.Add(entity);

        var e = (IInternalEntity)entity;
        e.Init(AddComponent, index);
    }

    public void UpdateComponent(int id, ITestComponent component)
    {
        _components[id] = component;
    }

    public ComponentProxy<TComponent> GetComponentRef<TComponent>(int id) where TComponent : ITestComponent
    {
        var component = (TComponent)_components[id];
        return new ComponentProxy<TComponent>((id, v) => this.UpdateComponent(id, v), component, id);
    }

    public TComponent GetComponent<TComponent>(int id)
    {
        return (TComponent)_components[id];
    }

    public void RemoveComponent(ITestComponent component)
    {
        _components.Remove(component.Id);
    }

    public void Update()
    {
        foreach (var system in _systems.ToSpan())
        {
            system.Update();
        }

        foreach (var component in _components.Values())
        {
            foreach (var system in _systems.ToSpan())
            {
                if (component.GetType() == system.CompType)
                {
                    _components[component.Id] = system.Update(component);
                }
            }
        }
    }
}
