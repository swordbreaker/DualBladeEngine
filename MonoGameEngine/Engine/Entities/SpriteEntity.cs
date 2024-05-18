using MonoGameEngine.Engine.Components;

namespace MonoGameEngine.Engine.Entities;

public class SpriteEntity : TransformEntity
{
    public RenderComponent Renderer { get; init; }

    public SpriteEntity()
    {
        Renderer = AddComponent<RenderComponent>();
    }
}
