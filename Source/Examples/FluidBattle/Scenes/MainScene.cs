using DualBlade.Core.Extensions;
using DualBlade.Core.Scenes;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using FluidBattle.Entities;
using FluidBattle.Systems;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FluidBattle.Scenes;

public class MainScene(IGameContext context) : GameScene(context)
{
    private readonly Random random = new();

    public override IEnumerable<ISystem> SetupSystems()
    {
        yield return new FollowCursorFluidSystem(context);
        yield return new FluidPixelSystem(context);
        yield return new FluidSystem(context);
    }

    protected override IEnumerable<EntityBuilder> SetupEntities()
    {
        var worldToPixel = GameContext.GameEngine.WorldToPixelConverter;

        var radius = 6f / worldToPixel.TileSize;
        var scale = 4;

        for (var i = 0; i < 300; i++)
        {
            var fluidPixelEntity = new FluidEntity(
                GameContext,
                new Vector2(random.NextFloat(-1, 1), random.NextFloat(-1, 1)),
                radius,
                scale);

            yield return CreateEntity(fluidPixelEntity)
                .AddChildren(Enumerable.Range(0, 8).Select(x => new EntityBuilder(new FluidPixelEntity(GameContext, scale, radius))));
        }

        yield return CreateEntity(new WallEntity(Vector2.Zero, new Vector2(10, 50), GameContext));
    }
}
