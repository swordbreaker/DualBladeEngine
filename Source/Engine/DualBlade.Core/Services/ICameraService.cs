namespace DualBlade.Core.Services;

public interface ICameraService
{
    Matrix TransformMatrix { get; }
    Matrix PixelTransformMatrix { get; }
}