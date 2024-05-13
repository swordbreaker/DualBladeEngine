using Microsoft.Xna.Framework;
using TestMonoGamesProject.Engine.Components;
using TestMonoGamesProject.Engine.Systems;
using TestMonoGamesProject.Engine.Worlds;

namespace TestMonoGamesProject.Systems
{
    public class RenderSystem : ComponentSystem<RenderComponent>
    {
        protected override void Draw(RenderComponent component, GameTime gameTime, IGameEngine gameEngine)
        {
            var transfrom = GetTransformComponent(component);
            gameEngine.Draw(
                component.Texture,
                transfrom.Position,
                component.Color,
                scale: transfrom.Scale,
                rotation: transfrom.Rotation);
        }
    }
}
