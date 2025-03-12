using DualBlade._3D.Rendering.Components;
using DualBlade.Core.Entities;
using Microsoft.Xna.Framework.Graphics;

namespace Example.ThreeDTest.Entities;

public partial struct ModelEntity : IEntity
{
    public ModelEntity(Model model, Vector3? position = null, Quaternion? rotation = null, Vector3? scale = null)
    {
        var transform = new TransformComponent3D
        {
            Position = position ?? Vector3.Zero,
            Rotation = rotation ?? Quaternion.Identity,
            Scale = scale ?? Vector3.One
        };
        var modelComponent = new ModelComponent { Model = model };
        AddComponent(transform);
        AddComponent(modelComponent);
    }
}
