using Microsoft.Xna.Framework.Graphics;

namespace TestMonoGamesProject.Engine.Physics
{
    public interface IColliderFactory
    {
        ICollider CreateRectColliderFormTexture(Texture2D texture);
    }
}