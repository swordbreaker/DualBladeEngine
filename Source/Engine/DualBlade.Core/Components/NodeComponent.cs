using DualBlade.Core.Collections;
using DualBlade.Core.Entities;

namespace DualBlade.Core.Components;

public class NodeComponent : INodeComponent
{
    public int? Parent { get; set; } = null;
    public GrowableMemory<int> Children { get; set; } = new(10);

    public IEntity Entity { get; init; }
}
