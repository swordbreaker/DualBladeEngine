using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using FluidBattle.Components;
using FluidBattle.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FluidBattle.Systems;

public class AiCursorFluidSystem(IGameContext context) : ComponentSystem<FluidComponent, TransformComponent>(context)
{
    private IWorldToPixelConverter worldToPixelConverter = context.GameEngine.WorldToPixelConverter;
    private Random random = new();
    private AiAgent _aiAgent = context.ServiceProvider.GetRequiredService<AiAgent>();

    protected override void Update(ref FluidComponent fluid, ref TransformComponent transform, ref IEntity entity, GameTime gameTime)
    {
        if (fluid.Player > 0)
        {
            fluid.Target = _aiAgent.FluidTarget;
        }
    }
}
