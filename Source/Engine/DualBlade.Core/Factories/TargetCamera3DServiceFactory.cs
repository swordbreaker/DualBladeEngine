using DualBlade.Core.Services;

namespace DualBlade.Core.Factories;

public class TargetCamera3DServiceFactory : ICameraServiceFactory
{
    public ICameraService Create(
        GraphicsDeviceManager graphicsDeviceManager,
        IWorldToPixelConverter worldToPixelConverter) =>
        new TargetCamera3DService(graphicsDeviceManager, worldToPixelConverter);
}