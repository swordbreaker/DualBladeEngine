using DualBlade.Core.Components;

namespace DualBlade._2D.Rendering.Components;

public partial struct TransformComponent : IComponent
{
    public Vector2 Position = Vector2.Zero;

    /// <summary>
    /// Rotation in degrees.
    /// Positive values rotate clockwise, negative values rotate counter-clockwise.
    /// </summary>
    public float Rotation = 0f;
    public Vector2 Scale = Vector2.One;
}