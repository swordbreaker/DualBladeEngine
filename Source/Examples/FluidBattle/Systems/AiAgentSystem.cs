using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using FluidBattle.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FluidBattle.Systems;
public class AiAgentSystem : BaseSystem
{
    private AiAgent agent;

    public AiAgentSystem(IGameContext gameContext) : base(gameContext)
    {
        agent = gameContext.ServiceProvider.GetRequiredService<AiAgent>();
    }

    public override void Update(GameTime gameTime)
    {
        agent.Update(gameTime);
    }
}
