using DualBlade.Core.Rendering;
using DualBlade.Core.Services;
using Microsoft.Xna.Framework.Graphics;

namespace DualBlade.Core.Factories;

public sealed class SpriteFactory(IWorldToPixelConverter _worldToPixelConverter, GraphicsDeviceManager graphicsDeviceManager) : ISpriteFactory
{
    private Texture2D? _whitePixelTexture;

    public ISprite CreateSprite(Texture2D texture2D) =>
        new Sprite(_worldToPixelConverter) { Texture2D = texture2D };

    public ISprite CreateWhitePixelSprite()
    {
        if (_whitePixelTexture is null)
        {
            _whitePixelTexture = new Texture2D(graphicsDeviceManager.GraphicsDevice, 1, 1, mipmap: false, SurfaceFormat.Color);
            _whitePixelTexture.SetData(new Color[1] { Color.White });
        }

        return new Sprite(_worldToPixelConverter) { Texture2D = _whitePixelTexture };
    }
}
