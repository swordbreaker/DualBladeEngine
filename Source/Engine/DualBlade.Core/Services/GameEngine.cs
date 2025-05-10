using DualBlade.Core.Rendering;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DualBlade.Core.Services;

public sealed record GameEngine(ISpriteFactory SpriteFactory) : IGameEngine
{
    public required IWorldToPixelConverter WorldToPixelConverter { get; init; }
    public required GraphicsDeviceManager GraphicsDeviceManager { get; init; }
    public required IInputManager InputManager { get; init; }
    public required ContentManager Content { get; init; }
    public SpriteBatch? SpriteBatch { get; private set; }
    public required ICameraService CameraService { get; init; }

    public Vector2 GameSize =>
        WorldToPixelConverter.PixelSizeToWorld(
            new(GraphicsDeviceManager.PreferredBackBufferWidth, GraphicsDeviceManager.PreferredBackBufferHeight));

    public void Initialize()
    {
        SpriteBatch = new SpriteBatch(GraphicsDeviceManager.GraphicsDevice);
    }

    public void BeginDraw()
    {
        if (SpriteBatch is null)
            throw new InvalidOperationException("SpriteBatch is not initialized");

        var pm = CameraService.ProjectionMatrix * CameraService.ViewMatrix;
        SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, transformMatrix: pm);
    }

    public void Draw(
        Texture2D texture,
        Vector2 position,
        Color? color = null,
        Rectangle? sourceRectangle = null,
        float rotation = 0f,
        Vector2? origin = null,
        Vector2? scale = null,
        SpriteEffects effects = SpriteEffects.None,
        float layerDepth = 0f)
    {
        color ??= Color.White;
        origin ??= new Vector2(texture.Width / 2f, texture.Height / 2f);
        scale ??= Vector2.One;

        if (SpriteBatch is null)
            throw new InvalidOperationException("SpriteBatch is not initialized");

        position = WorldToPixelConverter.WorldPointToPixel(position);
        origin = WorldToPixelConverter.WorldSizeToPixel(origin.Value);

        // Convert sourceRectangle from world to pixel space if provided
        if (sourceRectangle.HasValue && sourceRectangle.Value is Rectangle rect)
        {
            var location = WorldToPixelConverter.WorldPointToPixel(new Vector2(rect.X, rect.Y));
            var size = WorldToPixelConverter.WorldSizeToPixel(new Vector2(rect.Width, rect.Height));
            sourceRectangle = new Rectangle((int)location.X, (int)location.Y, (int)size.X, (int)size.Y);
        }

        SpriteBatch.Draw(
            texture,
            position,
            sourceRectangle: sourceRectangle,
            color: color.Value,
            rotation: rotation,
            origin: origin.Value,
            scale: scale.Value,
            effects: effects,
            layerDepth: layerDepth);
    }

    public void DrawString(
        SpriteFont spriteFont,
        string text,
        Vector2 position,
        Color color,
        float rotation = 0f,
        Vector2? origin = null,
        Vector2? scale = null,
        SpriteEffects effects = SpriteEffects.None,
        float layerDepth = 0f,
        bool rtl = false
    )
    {
        if (SpriteBatch is null)
            throw new InvalidOperationException("SpriteBatch is not initialized");

        origin ??= new Vector2(spriteFont.MeasureString(text).X / 2f, spriteFont.MeasureString(text).Y / 2f);
        scale ??= Vector2.One;

        position = WorldToPixelConverter.WorldPointToPixel(position);
        origin = WorldToPixelConverter.WorldSizeToPixel(origin.Value);

        SpriteBatch.DrawString(spriteFont, text, position, color);

        // SpriteBatch.DrawString(
        //     spriteFont,
        //     text,
        //     position,
        //     color,
        //     rotation,
        //     origin.Value,
        //     scale.Value,
        //     effects,
        //     layerDepth,
        //     rtl);
    }

    public void EndDraw()
    {
        if (SpriteBatch is null)
            throw new InvalidOperationException("SpriteBatch is not initialized");

        SpriteBatch.End();
    }

    public T Load<T>(string assetName) =>
        Content.Load<T>(assetName);

    public ISprite CreateSprite(string assetName)
    {
        var texture = Content.Load<Texture2D>(assetName);
        return SpriteFactory.CreateSprite(texture);
    }

    public ISprite CreateSprite(Texture2D texture2D) =>
        SpriteFactory.CreateSprite(texture2D);
}