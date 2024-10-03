using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using FluidBattle.Components;
using FluidBattle.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Color = Microsoft.Xna.Framework.Color;

namespace FluidBattle.Entities;

public partial struct FluidPixelEntity : IEntity
{
    public FluidPixelEntity(IGameContext context, float scale, float radius, Color color)
    {
        var circleSample = context.ServiceProvider.GetRequiredService<ICircleSampler>();

        var minRadius = 0.05f;
        var maxRadius = radius + 0.1f;

        var fluidPixel = new FluidPixelComponent() { MinRadius = minRadius, MaxRadius = maxRadius };

        var spriteFactory = context.GameEngine.SpriteFactory;

        var renderComponent = new RenderComponent();
        renderComponent.SetSprite(spriteFactory.CreateWhitePixelSprite());
        renderComponent.Color = color;

        var pos = circleSample.Sample(minRadius, maxRadius);

        var transform = new TransformComponent() { Scale = Vector2.One * scale, Position = pos };
        fluidPixel.Target = pos;

        AddComponent(transform);
        AddComponent(renderComponent);
        AddComponent(fluidPixel);
    }
}
