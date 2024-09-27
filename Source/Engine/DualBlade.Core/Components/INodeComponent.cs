using DualBlade.Core.Collections;

namespace DualBlade.Core.Components;
public interface INodeComponent : IComponent
{
    int? Parent { get; set; }
    GrowableMemory<int> Children { get; set; }
}
