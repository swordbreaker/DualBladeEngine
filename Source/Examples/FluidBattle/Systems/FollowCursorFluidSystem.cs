using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using FluidBattle.Components;
using Microsoft.Xna.Framework;

namespace FluidBattle.Systems;
public class FollowCursorFluidSystem(IGameContext context) : ComponentSystem<FluidComponent>(context)
{

    public override void Update(GameTime gameTime)
    {

    }

    public override void LateUpdate(GameTime gameTime)
    {

    }

    protected override void Update(ref FluidComponent component, ref IEntity entity, GameTime gameTime)
    {
        var mousePos = context.GameEngine.InputManager.MousePos;
        component.Target = GameContext.GameEngine.WorldToPixelConverter.PixelPointToWorld(mousePos);
    }
}
