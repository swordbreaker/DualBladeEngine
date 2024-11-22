using System.Drawing;

namespace DualBlade.Core.Services;

public sealed class WorldToPixelConverter(GraphicsDeviceManager graphicsDeviceManager) : IWorldToPixelConverter
{
    /// <summary>
    /// The size of a tile in pixels.
    /// </summary>
    public int TileSize { get; init; } = 32;

    public Matrix WorldMatrix =>
        Matrix.CreateScale(TileSize, -TileSize, TileSize) *
        Matrix.CreateTranslation(
            graphicsDeviceManager.PreferredBackBufferWidth * 0.5f,
            graphicsDeviceManager.PreferredBackBufferHeight * 0.5f,
            0);

    public RectangleF WorldBounds
    {
        get
        {
            var w = graphicsDeviceManager.PreferredBackBufferWidth / (float)TileSize;
            var h = graphicsDeviceManager.PreferredBackBufferHeight / (float)TileSize;
            return new RectangleF(-w / 2, -h / 2, w, h);
        }
    }

    public Matrix WorldSizeMatrix => Matrix.CreateScale(TileSize);

    public Vector2 WorldPointToPixel(Vector2 worldCoordinate) => Vector2.Transform(worldCoordinate, WorldMatrix);

    public Vector2 WorldSizeToPixel(Vector2 worldSize) => Vector2.Transform(worldSize, WorldSizeMatrix);

    public Vector2 PixelPointToWorld(Vector2 pixelCoordinate) =>
        Vector2.Transform(pixelCoordinate, Matrix.Invert(WorldMatrix));

    public Vector2 PixelSizeToWorld(Vector2 pixelSize) => Vector2.Transform(pixelSize, Matrix.Invert(WorldSizeMatrix));
}