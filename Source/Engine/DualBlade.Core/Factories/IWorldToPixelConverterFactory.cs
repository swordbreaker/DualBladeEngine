using DualBlade.Core.Services;

namespace DualBlade.Core.Factories;

public interface IWorldToPixelConverterFactory
{
    IWorldToPixelConverter Create(GraphicsDeviceManager graphicsDeviceManager);
}