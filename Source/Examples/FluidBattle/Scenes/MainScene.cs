using DualBlade.Core.Extensions;
using DualBlade.Core.Scenes;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using FluidBattle.Entities;
using FluidBattle.Systems;
using System.Collections.Generic;
using System.Linq;

namespace FluidBattle.Scenes;

public class MainScene(IGameContext context) : GameScene(context)
{
    private readonly Random random = new();

    public override IEnumerable<ISystem> SetupSystems()
    {
        yield return new FollowCursorFluidSystem(GameContext);
        yield return new FluidPixelSystem(GameContext);
        yield return new FluidSystem(GameContext);
        yield return new AiCursorFluidSystem(GameContext);
        yield return new AiAgentSystem(GameContext);
        yield return new PlayerCursorSystem(GameContext);
    }

    protected override IEnumerable<EntityBuilder> SetupEntities()
    {
        var worldToPixel = GameContext.GameEngine.WorldToPixelConverter;

        var radius = 6f / worldToPixel.TileSize;
        var scale = 4;

        for (var i = 0; i < 300; i++)
        {
            var fluidEntity = new FluidEntity(
                GameContext,
                new Vector2(random.NextFloat(-1, 1), random.NextFloat(-1, 1)),
                radius,
                scale,
                0,
                Color.Red);

            yield return CreateEntity(fluidEntity)
                .AddChildren(Enumerable.Range(0, 8).Select(x => new EntityBuilder(new FluidPixelEntity(GameContext, scale, radius, Color.Red))));
        }

        for (var i = 0; i < 100; i++)
        {
            var fluidEntity = new FluidEntity(
                GameContext,
                new Vector2(random.NextFloat(4, 5), random.NextFloat(-1, 1)),
                radius,
                scale,
                1,
                Color.Purple);

            var fluidComponent = fluidEntity.FluidComponentCopy;
            fluidComponent.Target = new Vector2(-4, 0);
            fluidEntity.UpdateComponent(fluidComponent);

            yield return CreateEntity(fluidEntity)
                .AddChildren(Enumerable.Range(0, 8).Select(x => new EntityBuilder(new FluidPixelEntity(GameContext, scale, radius, Color.Purple))));
        }

        yield return CreateEntity(new WallEntity(Vector2.Zero, new Vector2(10, 50), GameContext));
        yield return CreateEntity(new PlayerCursorEntity(GameContext));
    }
}
