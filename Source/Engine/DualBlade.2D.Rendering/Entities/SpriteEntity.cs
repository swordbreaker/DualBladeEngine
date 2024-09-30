using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Rendering;

namespace DualBlade._2D.Rendering.Entities;

public partial struct SpriteEntity : IEntity
{
    public SpriteEntity(ISprite? sprite = null)
    {
        var renderComponent = new RenderComponent();

        if (sprite is not null)
        {
            renderComponent.SetSprite(sprite);
        }

        AddComponent(new TransformComponent());
        AddComponent(renderComponent);
    }
}
