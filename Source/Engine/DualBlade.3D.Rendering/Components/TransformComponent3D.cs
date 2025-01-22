using DualBlade.Core.Components;
using Microsoft.Xna.Framework;

namespace DualBlade._3D.Rendering.Components;

public partial struct TransformComponent3D : IComponent
{
    public Vector3 Position = Vector3.Zero;
    public float Rotation = 0f;
    public Vector3 Scale = Vector3.One;
}