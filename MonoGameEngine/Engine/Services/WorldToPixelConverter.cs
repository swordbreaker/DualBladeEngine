namespace MonoGameEngine.Engine.Services;
public class WorldToPixelConverter(GraphicsDeviceManager _graphicsDeviceManager) : IWorldToPixelConverter
{
    /// <summary>
    /// The size of a tile in pixels.
    /// </summary>
    public int TileSize { get; init; } = 32;

    public Matrix WorldMatrix =>
        Matrix.CreateScale(TileSize, -TileSize, TileSize) *
        Matrix.CreateTranslation(
            _graphicsDeviceManager.PreferredBackBufferWidth * 0.5f,
            _graphicsDeviceManager.PreferredBackBufferHeight * 0.5f,
            0);

    public Matrix WorldSizeMatrix => Matrix.CreateScale(TileSize);

    public Vector2 WorldPointToPixel(Vector2 worldCoordinate) => Vector2.Transform(worldCoordinate, WorldMatrix);

    public Vector2 WorldSizeToPixel(Vector2 worldSize) => Vector2.Transform(worldSize, WorldSizeMatrix);

    public Vector2 PixelPointToWorld(Vector2 pixelCoordinate) => Vector2.Transform(pixelCoordinate, Matrix.Invert(WorldMatrix));

    public Vector2 PixelSizeToWorld(Vector2 pixelSize) => Vector2.Transform(pixelSize, Matrix.Invert(WorldSizeMatrix));
}