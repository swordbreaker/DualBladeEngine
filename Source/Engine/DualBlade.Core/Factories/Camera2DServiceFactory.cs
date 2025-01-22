using DualBlade.Core.Services;

namespace DualBlade.Core.Factories;

public sealed class Camera2DServiceFactory : ICameraServiceFactory
{
    public ICameraService Create(GraphicsDeviceManager graphicsDeviceManager,
        IWorldToPixelConverter worldToPixelConverter) =>
        new Camera2DService(graphicsDeviceManager, worldToPixelConverter);
}