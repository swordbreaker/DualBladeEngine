using DualBlade.Core.Collections;
using DualBlade.Core.Components;
using DualBlade.Core.Worlds;

namespace DualBlade.Core.Entities;

public interface IEntity
{
    public int Id { get; }

    ComponentRef<TComponent>? Component<TComponent>() where TComponent : IComponent;

    GrowableMemory<ComponentRef<IComponent>> Components { get; }

    GrowableMemory<IComponent> InitialComponents { get; }

    void AddComponent(IComponent component);

    void Init(
        World.AddComponentDelegate addComponent,
        int id);
}