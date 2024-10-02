using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Rendering;

namespace DualBlade._2D.Rendering.Entities;

[AddComponent<TransformComponent>]
public partial struct SpriteEntity : IEntity
{
    public SpriteEntity(ISprite? sprite = null)
    {
        var renderComponent = new RenderComponent();

        if (sprite is not null)
        {
            renderComponent.SetSprite(sprite);
        }

        AddComponent(renderComponent);
    }
}
