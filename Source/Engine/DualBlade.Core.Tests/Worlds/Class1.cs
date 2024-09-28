using DualBlade.Core.Collections;
using DualBlade.Core.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Worlds;

namespace DualBlade.Core.Tests.Worlds;

public partial struct Class1 : INodeEntity
{
    public Class1() { }
    public INodeComponent NodeComponent { get; init; }
    public IEnumerable<INodeEntity> Children { get; } = new List<INodeEntity>();
    public INodeEntity? Parent { get; }

    public void AddChild(INodeEntity child) => throw new NotImplementedException();
    public void AddParent(INodeEntity parent) => throw new NotImplementedException();

    private GrowableMemory<(Type, int)> componentsIds = new(100);
    private World.AddComponentDelegate? addComponent;

    public int Id { get; private set; }
    public GrowableMemory<ComponentRef<IComponent>> Components { get; }
    public GrowableMemory<IComponent> InitialComponents { get; }

    public readonly void AddComponent(IComponent component)
    {
        if (componentsIds.Contains(x => x.Item1 == component.GetType()))
        {
            throw new InvalidOperationException("Component already added");
        }

        if (this.addComponent is not null)
        {
            this.Components.Add(this.addComponent(component));
        }
        else
        {
            this.InitialComponents.Add(component);
        }
    }

    public readonly ComponentRef<TComponent> Component<TComponent>() where TComponent : IComponent
    {
        var cRef = this.Components.Find(x => x.GetCopy().GetType() == typeof(TComponent));
        return cRef.As<TComponent>();
    }

    public void Init(World.AddComponentDelegate addComponent, int id)
    {
        this.Id = id;
        this.addComponent = addComponent;
    }
}
