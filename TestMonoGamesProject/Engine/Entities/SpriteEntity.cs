using TestMonoGamesProject.Engine.Components;

namespace TestMonoGamesProject.Engine.Entities;

public class SpriteEntity : TransformEntity
{
    public RenderComponent Renderer { get; init; }

    public SpriteEntity()
    {
        Renderer = AddComponent<RenderComponent>();
    }
}
