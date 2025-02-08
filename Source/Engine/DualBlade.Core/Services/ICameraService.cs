namespace DualBlade.Core.Services;

/// <summary>
/// Represents a camera service that provides view and projection matrices.
/// </summary>
public interface ICameraService
{
    /// <summary>
    /// The view matrix transforms vertices from world space to camera space (also called view space).
    /// It essentially represents the camera's position and orientation in the world.
    /// </summary>
    Matrix ViewMatrix { get; }

    /// <summary>
    /// The projection matrix is responsible for transforming vertices from camera space to clip space,
    /// preparing them for the final step of rendering on a 2D screen.
    /// </summary>
    Matrix ProjectionMatrix { get; }
}