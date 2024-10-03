using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;

namespace FluidBattle.Entities;

[RequiredComponent<RenderComponent>]
[RequiredComponent<TransformComponent>]
public partial struct PlayerCursorEntity : IEntity
{
    public PlayerCursorEntity(IGameContext context)
    {
        var renderer = new RenderComponent();
        renderer.SetSprite(context.GameEngine.CreateSprite("PlayerCursor"));
        renderer.Color = Color.Green;

        var transform = new TransformComponent
        {
            Scale = One
        };

        AddComponent(renderer);
        AddComponent(transform);
    }
}
