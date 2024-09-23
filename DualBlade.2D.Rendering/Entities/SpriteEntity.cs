using DualBlade._2D.Rendering.Components;

namespace DualBlade._2D.Rendering.Entities;

public class SpriteEntity : TransformEntity
{
    public RenderComponent Renderer { get; init; }

    public SpriteEntity()
    {
        Renderer = AddComponent<RenderComponent>();
    }
}
