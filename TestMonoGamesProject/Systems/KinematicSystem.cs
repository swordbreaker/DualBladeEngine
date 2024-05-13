using Microsoft.Xna.Framework;
using TestMonoGamesProject.Components;
using TestMonoGamesProject.Engine.Components;
using TestMonoGamesProject.Engine.Systems;
using TestMonoGamesProject.Engine.Worlds;

namespace TestMonoGamesProject.Systems
{
    public class KinematicSystem : ComponentSystem<KinematicComponent>
    {
        protected override void Update(KinematicComponent component, GameTime gameTime, IGameEngine gameEngine)
        {
            var entity = component.Entity;
            var transform = this.World.GetComponent<TransformComponent>(entity)!;
            var colliderComponent = this.World.GetComponent<ColliderComponent>(entity);

            var kinematicVelocity = component.KinematicVelocity;
            var velocity = component.Velocity + kinematicVelocity;
            var posX = transform.Position.X + component.Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
            var posY = transform.Position.Y + component.Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (colliderComponent is not null)
            {
                var collider = colliderComponent.Collider;

                var colliderX = collider.SetLocation(new Vector2(posX, 0));
                var colliderY = collider.SetLocation(new Vector2(0, posY));
                if (gameEngine.PhysicsManager.CheckCollision(colliderX))
                {
                    velocity.X = 0;
                    kinematicVelocity.X = 0;
                }

                if (gameEngine.PhysicsManager.CheckCollision(colliderY))
                {
                    velocity.Y = 0;
                    kinematicVelocity.Y = 0;
                }
            }

            transform!.Position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            component.Velocity = velocity;
            component.KinematicVelocity = kinematicVelocity + component.Acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
