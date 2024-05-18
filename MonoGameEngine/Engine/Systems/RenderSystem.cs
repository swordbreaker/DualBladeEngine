using MonoGameEngine.Engine.Components;
using MonoGameEngine.Engine.Extensions;
using MonoGameEngine.Engine.Worlds;

namespace MonoGameEngine.Engine.Systems;

public class RenderSystem : ComponentSystem<RenderComponent>
{
    protected override void Draw(RenderComponent component, GameTime gameTime, IGameEngine gameEngine)
    {
        var transform = GetTransformComponent(component);
        var origin = component.Origin;
        var rotation = MathHelper.ToRadians(transform.AbsoluteRotation());

        gameEngine.Draw(
            component.Texture!,
            transform.AbsolutePosition(),
            component.Color,
            scale: transform.AbsoluteScale(),
            rotation: rotation,
            origin: origin);
    }
}
