using Microsoft.Xna.Framework;
using TestMonoGamesProject.Engine.Components;
using TestMonoGamesProject.Engine.Worlds;

namespace TestMonoGamesProject.Engine.Systems
{
    public class KinematicSystem : ComponentSystem<KinematicComponent>
    {
        protected override void Update(KinematicComponent component, GameTime gameTime, IGameEngine gameEngine)
        {
            var entity = component.Entity;
            var transform = World.GetComponent<TransformComponent>(entity)!;

            transform.Position = component.PhysicsBody.Position;
        }
    }
}
