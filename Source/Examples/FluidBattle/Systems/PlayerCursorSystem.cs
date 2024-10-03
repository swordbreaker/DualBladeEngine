using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using FluidBattle.Entities;

namespace FluidBattle.Systems;
public class PlayerCursorSystem(IGameContext context) : EntitySystem<PlayerCursorEntity>(context)
{
    private IInputManager _inputManger = context.GameEngine.InputManager;
    private IWorldToPixelConverter _worldToPixelConverter = context.GameEngine.WorldToPixelConverter;

    protected override void Update(ref PlayerCursorEntity entity, GameTime gameTime)
    {
        var transform = entity.TransformComponentCopy;
        transform.Position = _worldToPixelConverter.PixelPointToWorld(_inputManger.MousePos);
        entity.UpdateComponent(transform);
    }
}
