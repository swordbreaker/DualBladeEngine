
namespace MonoGameEngine.Engine.Services;

public interface IWorldToPixelConverterFactory
{
    IWorldToPixelConverter Create(GraphicsDeviceManager graphicsDeviceManager);
}