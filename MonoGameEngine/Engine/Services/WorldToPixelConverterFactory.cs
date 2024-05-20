namespace MonoGameEngine.Engine.Services;

public class WorldToPixelConverterFactory : IWorldToPixelConverterFactory
{
    public IWorldToPixelConverter Create(GraphicsDeviceManager graphicsDeviceManager) =>
        new WorldToPixelConverter(graphicsDeviceManager);
}
