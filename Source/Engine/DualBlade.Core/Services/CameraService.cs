using DualBlade.Core.Extensions;

namespace DualBlade.Core.Services;

internal sealed class CameraService(GraphicsDeviceManager _graphicsDeviceManager, IWorldToPixelConverter _worldToPixelConverter) : ICameraService
{
    private Vector2 GameSize => new(_graphicsDeviceManager.PreferredBackBufferWidth, _graphicsDeviceManager.PreferredBackBufferHeight);

    public Vector2 Position { get; set; } = Vector2.Zero;
    public float Zoom { get; set; } = 1.0f;
    public float Rotation { get; set; } = 0.0f;
    public Matrix TransformMatrix =>
        Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
        Matrix.CreateRotationZ(Rotation) *
        Matrix.CreateScale(Zoom);

    public Matrix PixelTransformMatrix =>
        Matrix.CreateTranslation(_worldToPixelConverter.WorldPointToPixel(-Position).ToVector3()) *
        Matrix.CreateTranslation(new Vector3(-GameSize / 2, 0)) *
        Matrix.CreateRotationZ(Rotation) *
        Matrix.CreateScale(Zoom);
}
