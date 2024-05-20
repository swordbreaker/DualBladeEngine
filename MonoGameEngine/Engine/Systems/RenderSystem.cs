using MonoGameEngine.Engine.Components;
using MonoGameEngine.Engine.Extensions;
using MonoGameEngine.Engine.Worlds;

namespace MonoGameEngine.Engine.Systems;

public class RenderSystem : ComponentSystem<RenderComponent>
{
    protected override void Draw(RenderComponent component, GameTime gameTime, IGameEngine gameEngine)
    {
        var transform = GetTransformComponent(component);
        var position = transform.AbsolutePosition();
        var origin = component.Origin;
        var rotation = MathHelper.ToRadians(transform.AbsoluteRotation());

        if(component.Sprite is null)
        {
            return;
        }

        gameEngine.Draw(
            component.Sprite.Texture2D,
            position,
            component.Color,
            scale: transform.AbsoluteScale(),
            rotation: rotation,
            origin: origin);
    }
}
