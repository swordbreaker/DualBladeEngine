using DualBlade.Core.Components;

namespace DualBlade._2D.Rendering.Components;

public class TransformComponent : NodeComponent
{
    public Vector2 Position = Vector2.Zero;
    public float Rotation = 0f;
    public Vector2 Scale = Vector2.One;
}
