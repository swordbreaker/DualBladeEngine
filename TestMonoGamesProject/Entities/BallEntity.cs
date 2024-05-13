using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing;
using TestMonoGamesProject.Components;
using TestMonoGamesProject.Engine.Entities;
using TestMonoGamesProject.Engine.Physics;
using TestMonoGamesProject.Engine.Worlds;

namespace TestMonoGamesProject.Entities
{
    public class BallEntity : SpriteEntity
    {
        public static float G = 9.81f * 50;

        public KinematicComponent KinematicComponent { get; }

        public BallEntity(IGameEngine gameEngine, Vector2 pos)
        {
            KinematicComponent = AddComponent<KinematicComponent>();
            var colliderComponent = AddComponent<ColliderComponent>();

            Transform!.Position = pos;
            KinematicComponent.Acceleration = new Vector2(0, G);
            Renderer.SetTexture(gameEngine.Load<Texture2D>("ball"));
            colliderComponent.Collider = new RectCollider(new RectangleF(new PointF(pos.X, pos.Y), new SizeF(32, 32)));
        }
    }
}
