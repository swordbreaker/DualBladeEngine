using DualBlade.Core.Extensions;
using DualBlade.Core.Scenes;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using FluidBattle.Entities;
using FluidBattle.Factories;
using FluidBattle.Systems;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace FluidBattle.Scenes;

public class MainScene(IGameContext context) : GameScene(context)
{
    private readonly Random random = new();
    private readonly FluidFactory fluidFactory = context.ServiceProvider.GetRequiredService<FluidFactory>();

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

        foreach (var fluid in fluidFactory.CreateFluids(20, new Vector2(-2, 0), 0.5f, 0, Color.Red))
        {
            yield return fluid;
        }

        foreach (var fluid in fluidFactory.CreateFluids(100, new Vector2(2, 0), 0.5f, 1, Color.Purple))
        {
            yield return fluid;
        }

        for (var i = 0; i < 20; i++)
        {
            var x = random.NextFloat(-5, 5);
            var y = random.NextFloat(-4, 4);

            yield return fluidFactory.CreateFluid(new Vector2(x, y), -1, Color.Gray);
        }

        //yield return CreateEntity(new WallEntity(Vector2.Zero, new Vector2(10, 50), GameContext));
        yield return CreateEntity(new PlayerCursorEntity(GameContext));
    }
}
