using DualBlade.Core.Extensions;

namespace DualBlade.Core.Services;

internal sealed class Camera2DService(
    GraphicsDeviceManager graphicsDeviceManager,
    IWorldToPixelConverter worldToPixelConverter) : ICamera2DService
{
    private Vector2 PixelGameSize => new(graphicsDeviceManager.PreferredBackBufferWidth,
        graphicsDeviceManager.PreferredBackBufferHeight);

    public Vector2 GameSize =>
        worldToPixelConverter.PixelSizeToWorld(PixelGameSize);

    public Vector2 Position { get; set; } = Vector2.Zero;
    public float Zoom { get; set; } = 1.0f;
    public float Rotation { get; set; } = 0.0f;

    public Matrix ViewMatrix =>
        Matrix.CreateTranslation(worldToPixelConverter.WorldPointToPixel(-Position).ToVector3()) *
        Matrix.CreateTranslation(new Vector3(-PixelGameSize / 2, 0)) *
        Matrix.CreateRotationZ(Rotation) *
        Matrix.CreateScale(Zoom);

    public Matrix ProjectionMatrix =>
        Matrix.Identity;
}