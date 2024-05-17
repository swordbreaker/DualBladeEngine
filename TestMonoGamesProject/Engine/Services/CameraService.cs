using Microsoft.Xna.Framework;
using TestMonoGamesProject.Engine.Worlds;

namespace TestMonoGamesProject.Engine.Services;
public class CameraService
{
    private readonly GraphicsDeviceManager _graphicsDeviceManager;

    private Vector2 GameSize => new (_graphicsDeviceManager.PreferredBackBufferWidth, _graphicsDeviceManager.PreferredBackBufferHeight);

    public Vector2 Position { get; set; }
    public float Zoom { get; set; } = 1.0f;
    public float Rotation { get; set; }
    public Matrix TransformMatrix =>
        Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
        Matrix.CreateRotationZ(Rotation) *
        Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
        Matrix.CreateTranslation(new Vector3(GameSize.X * 0.5f, GameSize.Y * 0.5f, 0));

    public CameraService(GraphicsDeviceManager graphicsDeviceManager)
    {
        Position = Vector2.Zero;
        Zoom = 1.0f;
        Rotation = 0.0f;
        _graphicsDeviceManager = graphicsDeviceManager;
    }
}
