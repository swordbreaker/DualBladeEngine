using DualBlade.Core.Collections;
using DualBlade.Core.Components;
using DualBlade.Core.Worlds;

namespace DualBlade.Core.Entities;

public partial struct RootEntity : INodeEntity
{
    public RootEntity(IEnumerable<IEntity> children)
    {
        Children = children;
    }

    public INodeComponent NodeComponent { get; init; }
    public IEnumerable<IEntity> Children { get; }
    public INodeEntity? Parent { get; }
    public int Id { get; }
    public GrowableMemory<ComponentRef<IComponent>> Components { get; } = new();
    public GrowableMemory<IComponent> InitialComponents { get; } = new();

    public void AddChild(INodeEntity child) => throw new NotImplementedException();
    public void AddComponent(IComponent component) => throw new NotImplementedException();
    public void AddParent(INodeEntity parent) => throw new NotImplementedException();
    public ComponentRef<TComponent>? Component<TComponent>() where TComponent : IComponent => throw new NotImplementedException();
    public void Init(World.AddComponentDelegate addComponent, int id)
    {

    }
}