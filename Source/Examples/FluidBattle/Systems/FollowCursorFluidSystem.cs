using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using FluidBattle.Components;
using Microsoft.Xna.Framework;

namespace FluidBattle.Systems;
public class FollowCursorFluidSystem(IGameContext context) : ComponentSystem<FluidComponent>(context)
{
    private readonly IGameEngine gameEngine = context.GameEngine;

    protected override void Update(ref FluidComponent component, ref IEntity entity, GameTime gameTime)
    {
        if (component.Player != 0)
            return;

        var mousePos = gameEngine.InputManager.MousePos;
        component.Target = gameEngine.WorldToPixelConverter.PixelPointToWorld(mousePos);
    }
}
