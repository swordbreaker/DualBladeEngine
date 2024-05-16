using Microsoft.Xna.Framework.Graphics;
using nkast.Aether.Physics2D.Dynamics;
using TestMonoGamesProject.Engine.Components;
using TestMonoGamesProject.Engine.Entities;
using TestMonoGamesProject.Engine.Worlds;

namespace ExampleGame.Entities
{
    public class BallEntity : SpriteEntity
    {
        public static float G = 9.81f * 50;

        public KinematicComponent KinematicComponent { get; }

        public BallEntity(IGameEngine gameEngine)
        {
            KinematicComponent = AddComponent<KinematicComponent>();
            
            Transform!.Position = gameEngine.GameSize/2;
            Renderer.SetTexture(gameEngine.Load<Texture2D>("ball"));

            // Create a physics body
            var body = gameEngine.PhysicsManager.CreateBody(Transform.Position, bodyType: BodyType.Dynamic);
            body.IgnoreGravity = true;
            body.FixedRotation = true;
            var fixture = body.CreateCircle(Renderer.Texture.Width/2f, 1);
            fixture.Restitution = 0.5f;
            fixture.Friction = 1f;

            KinematicComponent.PhysicsBody = body;
        }
    }
}
