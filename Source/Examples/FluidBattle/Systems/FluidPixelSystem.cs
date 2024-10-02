using DualBlade._2D.Rendering.Components;
using DualBlade._2D.Rendering.Extensions;
using DualBlade.Core.Entities;
using DualBlade.Core.Extensions;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using FluidBattle.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;

namespace FluidBattle.Systems;
public class FluidPixelSystem(IGameContext context) : ComponentSystem<FluidPixelComponent, TransformComponent>(context)
{
    private static readonly Random Random = new();
    private static float Speed = 0.1f;

    private readonly SpriteBatch spriteBatch = context.GameEngine.SpriteBatch;

    protected override void Update(ref FluidPixelComponent flux, ref TransformComponent transform, ref IEntity entity, GameTime gameTime)
    {
        if (Vector2.Distance(transform.Position, flux.Target) < 0.01f)
        {
            var parent = Ecs.GetParent(entity);
            var parentTransform = parent.Value.Component<TransformComponent>().Value;

            var radius = Random.NextFloat(0.05f, flux.Radius + 0.1f);
            var angle = Random.NextFloat(0, MathHelper.TwoPi);
            flux.Target = new Vector2(MathF.Cos(angle), MathF.Sin(angle)) * radius;
        }

        var direction = flux.Target - transform.Position;
        transform.Position += direction * Speed * gameTime.DeltaSeconds();
    }
}
