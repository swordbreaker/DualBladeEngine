using DualBlade.Core.Collections;
using DualBlade.Core.Worlds;

namespace DualBlade.Core.Components;
public interface INodeComponent : IComponent
{
    ComponentRef<IComponent>? Parent { get; set; }
    GrowableMemory<ComponentRef<IComponent>> Children { get; }
}
