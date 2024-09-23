using DualBlade.Core.Services;

namespace DualBlade.Core.Factories;

public sealed class WorldToPixelConverterFactory : IWorldToPixelConverterFactory
{
    public IWorldToPixelConverter Create(GraphicsDeviceManager graphicsDeviceManager) =>
        new WorldToPixelConverter(graphicsDeviceManager);
}
