using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Extensions;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using FluidBattle.Components;
using FluidBattle.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FluidBattle.Systems;
public class FluidPixelSystem(IGameContext context) : ComponentSystem<FluidPixelComponent, TransformComponent>(context)
{
    private ICircleSampler _circleSampler = context.ServiceProvider.GetRequiredService<ICircleSampler>();

    private static readonly Random Random = new();
    private static float Speed = 0.1f;

    private readonly SpriteBatch spriteBatch = context.GameEngine.SpriteBatch;

    protected override void Update(ref FluidPixelComponent flux, ref TransformComponent transform, ref IEntity entity, GameTime gameTime)
    {
        if (Vector2.Distance(transform.Position, flux.Target) < 0.01f)
        {
            flux.Target = _circleSampler.Sample(flux.MinRadius, flux.MaxRadius);
        }

        var direction = Normalize(flux.Target - transform.Position);
        transform.Position += direction * Speed * gameTime.DeltaSeconds();
    }
}
