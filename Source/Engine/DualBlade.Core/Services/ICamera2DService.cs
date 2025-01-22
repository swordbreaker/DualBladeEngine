namespace DualBlade.Core.Services;

public interface ICamera2DService : ICameraService
{
    float Zoom { get; set; }
    Vector2 Position { get; set; }
    float Rotation { get; set; }
}