using DualBlade.Core.Extensions;
using DualBlade.Core.Services;
using FluidBattle.Entities;
using System.Collections.Generic;
using System.Linq;

namespace FluidBattle.Factories;

internal class FluidFactory(IGameContext gameContext)
{
    private const float scale = 4;
    private readonly float radius = 32f / gameContext.GameEngine.WorldToPixelConverter.TileSize; // TileSize: 32
    private readonly Random random = new();

    public EntityBuilder<FluidEntity> CreateFluid(Vector2 position, int player, Color color)
    {
        var fluidEntity = new FluidEntity(
            gameContext,
            position,
            radius,
            scale,
            player,
            color);

        var fluidComponent = fluidEntity.FluidComponentCopy;
        fluidComponent.Target = new Vector2(-4, 0);
        fluidEntity.UpdateComponent(fluidComponent);

        var builder = new EntityBuilder<FluidEntity>(fluidEntity);
        builder
            .AddChildren(Enumerable.Range(0, 20)
                .Select(x => new EntityBuilder(new FluidPixelEntity(gameContext, scale, radius, color))));

        return builder;
    }

    public IEnumerable<EntityBuilder> CreateFluids(int count, Vector2 position, float radius, int player, Color color)
    {
        for (var i = 0; i < count; i++)
        {
            var r = random.NextFloat(0, radius);
            var angle = random.NextFloat(0, MathF.PI * 2);
            var pos = position + new Vector2(r * MathF.Cos(angle), r * MathF.Sin(angle));

            yield return CreateFluid(pos, player, color);
        }
    }
}