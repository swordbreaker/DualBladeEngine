using DualBlade.Core.Rendering;
using Microsoft.Xna.Framework.Graphics;
namespace DualBlade.Core.Services;

public interface IGameEngine
{
    /// <summary>
    /// Gets the content manager.
    /// </summary>
    GraphicsDeviceManager GraphicsDeviceManager { get; }

    /// <summary>
    /// Gets the input manager.
    /// </summary>
    IInputManager InputManager { get; }

    ICameraService CameraService { get; init; }

    /// <summary>
    /// Initializes the game engine.
    /// </summary>
    void Initialize();

    /// <summary>
    /// Get the window game size.
    /// </summary>
    Vector2 GameSize { get; }

    IWorldToPixelConverter WorldToPixelConverter { get; init; }

    public SpriteBatch? SpriteBatch { get; }

    /// <summary>
    /// Begins the draw.
    /// </summary>
    void BeginDraw();

    /// <summary>
    /// Ends the draw.
    /// </summary>
    void EndDraw();

    /// <summary>
    /// Draw a texture.
    /// </summary>
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

    /// <summary>
    /// Load content.
    /// </summary>
    /// <typeparam name="T">The type of the content.</typeparam>
    /// <param name="assetName">The asset name.</param>
    T Load<T>(string assetName);

    ISprite CreateSprite(string assetName);
    ISprite CreateSprite(Texture2D texture2D);
}