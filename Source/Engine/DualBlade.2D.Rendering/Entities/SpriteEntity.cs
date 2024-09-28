﻿using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Rendering;
using DualBlade.Core.Worlds;

namespace DualBlade._2D.Rendering.Entities;

public partial struct SpriteEntity : IEntity
{
    public readonly ComponentRef<RenderComponent> RenderComponent
    {
        get
        {
            var c = this.Component<RenderComponent>();
            if (c.HasValue)
            {
                return c.Value;
            }
            throw new InvalidOperationException("Only access this property after the entity was added to the world");
        }
    }

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
