using DualBlade.Core.Components;
using DualBlade.Core.Worlds;

namespace DualBlade.Core.Entities;

public struct RootEntity : INodeEntity
{
    public INodeComponent NodeComponent { get => throw new NotImplementedException(); init => throw new NotImplementedException(); }

    public IEnumerable<IEntity> Children => throw new NotImplementedException();

    public IEntity? Parent => throw new NotImplementedException();

    public IEnumerable<IComponent> Components { get => throw new NotImplementedException(); init => throw new NotImplementedException(); }

    public void AddChild(IEntity child) => throw new NotImplementedException();
    public TComponent AddComponent<TComponent>() where TComponent : IComponent, new() => throw new NotImplementedException();
    public void AddComponent<TComponent>(TComponent component) where TComponent : IComponent => throw new NotImplementedException();
    public void AddParent(IEntity parent) => throw new NotImplementedException();
    public void Initialize(IWorld world) => throw new NotImplementedException();
    public void RemoveComponent<TComponent>() where TComponent : IComponent => throw new NotImplementedException();
    public void RemoveComponent(Type type) => throw new NotImplementedException();
}