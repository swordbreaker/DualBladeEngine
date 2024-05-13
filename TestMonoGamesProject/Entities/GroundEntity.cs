using Microsoft.Xna.Framework.Graphics;
using System.Drawing;
using TestMonoGamesProject.Components;
using TestMonoGamesProject.Engine.Entities;
using TestMonoGamesProject.Engine.Physics;
using TestMonoGamesProject.Engine.Worlds;

namespace TestMonoGamesProject.Entities;
public class GroundEntity : SpriteEntity
{
    public GroundEntity(IGameEngine gameEngine)
    {
        var (w, h) = gameEngine.GameSize;
        var collider = AddComponent<ColliderComponent>();
        var rect = new RectangleF(
                0,
                h - 1,
                w,
                1);

        collider.Collider = new RectCollider(rect);
        Renderer.SetTexture(new Texture2D(gameEngine.GraphicsDeviceManager.GraphicsDevice, (int)rect.Width, (int)rect.Height));
    }
}
