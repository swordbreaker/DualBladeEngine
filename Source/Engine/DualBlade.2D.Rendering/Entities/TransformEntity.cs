using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Worlds;

namespace DualBlade._2D.Rendering.Entities;

public partial struct TransformEntity : IEntity
{
    public readonly ComponentRef<TransformComponent> Transform
    {
        get
        {
            var c = this.Component<TransformComponent>();
            if (c.HasValue)
            {
                return c.Value;
            }
            throw new InvalidOperationException("Only access this property after the entity was added to the world");
        }
    }

    public TransformEntity()
    {
        AddComponent(new TransformComponent());
    }
}
