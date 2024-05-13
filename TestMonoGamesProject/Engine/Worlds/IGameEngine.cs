using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TestMonoGamesProject.Engine.Physics;

namespace TestMonoGamesProject.Engine.Worlds
{
    public interface IGameEngine
    {
        IPhysicsManager PhysicsManager { get; }

        GraphicsDeviceManager GraphicsDeviceManager { get; }

        InputManager InputManager { get; }

        void Initialize();
        Vector2 GameSize { get; }
        void BeginDraw();

        void EndDraw();
        void Draw(
            Texture2D texture,
            Vector2 position,
            Color? color = null,
            Rectangle? sourceRectangle = null,
            float rotation = 0f,
            Vector2? origin = null,
            Vector2? scale = null,
            SpriteEffects effects = SpriteEffects.None,
            float layerDepth = 0f);
        T Load<T>(string assetName);
    }
}