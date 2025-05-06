using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BulletHell.Desktop.Systems;

public class TestSystem(IGameContext gameContext) : BaseSystem(gameContext)
{
    private readonly IGameEngine gameEngine = gameContext.GameEngine;
    private readonly SpriteBatch spriteBatch = gameContext.GameEngine.SpriteBatch;

    private Texture2D _pixel;

    public override void Initialize()
    {
        _pixel = new(spriteBatch.GraphicsDevice, 1, 1);
        _pixel.SetData([Color.Black]);
    }

    public override void Draw(GameTime gameTime)
    {
        var color = Color.Gray;

        gameEngine.BeginDraw();
        gameEngine.Draw(_pixel, Vector2.Zero, color, sourceRectangle: new Rectangle(0, 0, 200, 200));

        gameEngine.EndDraw();
    }
}
