
namespace MonoGameEngine.Engine.Services;

public interface ICameraService
{
    Vector2 Position { get; set; }
    float Rotation { get; set; }
    Matrix TransformMatrix { get; }
    Matrix PixelTransformMatrix { get; }
    float Zoom { get; set; }
}