using Microsoft.Xna.Framework;
using MonoGameEngine.Engine.Components;
using MonoGameEngine.Engine.Extensions;
using MonoGameEngine.Engine.Worlds;

namespace MonoGameEngine.Engine.Systems;

public class RenderSystem : ComponentSystem<RenderComponent>
{
    protected override void Draw(RenderComponent component, GameTime gameTime, IGameEngine gameEngine)
    {
        var transfrom = GetTransformComponent(component);
        var origin = component.Origin;
        var rotation = MathHelper.ToRadians(transfrom.AbsoluteRotation());

        gameEngine.Draw(
            component.Texture,
            transfrom.AbsolutePosition(),
            component.Color,
            scale: transfrom.AbsoluteScale(),
            rotation: rotation,
            origin: origin);
    }
}
