using DualBlade.Core.Extensions;

namespace DualBlade.Core.Services;

public class TargetCamera3DService(
    GraphicsDeviceManager graphicsDeviceManager,
    IWorldToPixelConverter worldToPixelConverter) : ICamera3DService
{
    private Vector2 GameSize => new(
        graphicsDeviceManager.PreferredBackBufferWidth,
        graphicsDeviceManager.PreferredBackBufferHeight);

    public Matrix ViewMatrix =>
        Matrix.CreateTranslation(worldToPixelConverter.WorldPointToPixel(-Position.XY()).ToVector3()) *
        Matrix.CreateTranslation(new Vector3(-GameSize / 2, 0)) *
        Matrix.CreateLookAt(Position, Target, Up);

    public Matrix ProjectionMatrix =>
        Matrix.CreatePerspectiveFieldOfView(
            FieldOfView,
            AspectRatio,
            NearPlaneDistance,
            FarPlaneDistance);

    public Vector3 Position { get; set; } = new Vector3(0, 1, 1);
    public Vector3 Target { get; set; } = Vector3.Zero;
    public Vector3 Up { get; set; } = Vector3.Up;
    public float NearPlaneDistance { get; set; } = 0.1f;
    public float FarPlaneDistance { get; set; } = 1000f;
    public float FieldOfView { get; set; } = MathF.PI / 4;
    public float AspectRatio { get; set; } = 16f / 9f;
}

public interface ICamera3DService : ICameraService
{
    public Vector3 Up { get; }
    public float NearPlaneDistance { get; set; }
    public float FarPlaneDistance { get; set; }
    public float FieldOfView { get; set; }
    public float AspectRatio { get; set; }
}