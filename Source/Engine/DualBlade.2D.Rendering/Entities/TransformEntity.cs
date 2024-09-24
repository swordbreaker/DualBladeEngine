using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Components;
using DualBlade.Core.Entities;

namespace DualBlade._2D.Rendering.Entities;

public class TransformEntity : NodeEntity
{
    public TransformComponent Transform { get; init; }

    public TransformEntity()
    {
        RemoveComponent<NodeComponent>();
        Transform = AddComponent<TransformComponent>();
        NodeComponent = Transform;
    }
}
