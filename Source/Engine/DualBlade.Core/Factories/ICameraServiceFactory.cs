using DualBlade.Core.Services;

namespace DualBlade.Core.Factories;
public interface ICameraServiceFactory
{
    ICameraService Create(GraphicsDeviceManager graphicsDeviceManager, IWorldToPixelConverter worldToPixelConverter);
}
