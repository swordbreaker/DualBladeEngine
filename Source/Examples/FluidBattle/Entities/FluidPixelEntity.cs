using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using FluidBattle.Components;
using Microsoft.Xna.Framework;

namespace FluidBattle.Entities;

public partial struct FluidPixelEntity : IEntity
{
    public FluidPixelEntity(IGameContext context, float scale, float radius)
    {
        var fluidPixel = new FluidPixelComponent() { Radius = radius };

        var spriteFactory = context.GameEngine.SpriteFactory;

        var renderComponent = new RenderComponent();
        renderComponent.SetSprite(spriteFactory.CreateWhitePixelSprite());
        renderComponent.Color = Color.Red;

        var transform = new TransformComponent() { Scale = Vector2.One * scale };

        AddComponent(transform);
        AddComponent(renderComponent);
        AddComponent(fluidPixel);
    }
}
