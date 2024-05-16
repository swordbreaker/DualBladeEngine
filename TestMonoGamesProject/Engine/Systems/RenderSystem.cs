using Microsoft.Xna.Framework;
using TestMonoGamesProject.Engine.Components;
using TestMonoGamesProject.Engine.Extensions;
using TestMonoGamesProject.Engine.Worlds;

namespace TestMonoGamesProject.Engine.Systems
{
    public class RenderSystem : ComponentSystem<RenderComponent>
    {
        protected override void Draw(RenderComponent component, GameTime gameTime, IGameEngine gameEngine)
        {
            var transfrom = GetTransformComponent(component);
            var origin = component.Origin;

            gameEngine.Draw(
                component.Texture,
                transfrom.AbsolutePosition(),
                component.Color,
                scale: transfrom.AbsoluteScale(),
                rotation: transfrom.AbsoluteRotation(),
                origin: origin);
        }
    }
}
