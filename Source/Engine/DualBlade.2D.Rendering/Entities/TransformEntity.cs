using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Worlds;

namespace DualBlade._2D.Rendering.Entities;

public partial struct TransformEntity : IEntity
{
    public readonly ComponentProxy<TransformComponent> Transform => this.Component<TransformComponent>();

    public TransformEntity()
    {
        AddComponent(new TransformComponent());
    }
}
