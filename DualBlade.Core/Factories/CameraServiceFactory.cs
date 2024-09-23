using DualBlade.Core.Services;

namespace DualBlade.Core.Factories;
public class CameraServiceFactory : ICameraServiceFactory
{
    public ICameraService Create(GraphicsDeviceManager graphicsDeviceManager, IWorldToPixelConverter worldToPixelConverter) =>
        new CameraService(graphicsDeviceManager, worldToPixelConverter);
}