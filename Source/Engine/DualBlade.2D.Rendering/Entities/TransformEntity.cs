using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Worlds;

namespace DualBlade._2D.Rendering.Entities;

public partial struct TransformEntity : IEntity
{
    public readonly ComponentProxy<TransformComponent> Transform => this.Component<TransformComponent>();

    public readonly Vector2 Position
    {
        get => Transform.Value.Position;

        set
        {
            using var proxy = Transform;
            proxy.Value.Position = value;
        }
    }

    public readonly Vector2 Scale
    {
        get => Transform.Value.Scale;

        set
        {
            using var proxy = Transform;
            proxy.Value.Scale = value;
        }
    }

    public readonly float Rotation
    {
        get => Transform.Value.Rotation;

        set
        {
            using var proxy = Transform;
            proxy.Value.Rotation = value;
        }
    }

    public TransformEntity()
    {
        AddComponent(new TransformComponent());
    }
}
