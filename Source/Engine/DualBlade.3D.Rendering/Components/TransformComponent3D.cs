using DualBlade.Core.Components;

namespace DualBlade._3D.Rendering.Components;

public partial struct TransformComponent3D : IComponent
{
    public Vector3 Position = Vector3.Zero;
    public Quaternion Rotation = Quaternion.Identity;
    public Vector3 Scale = Vector3.One;
}