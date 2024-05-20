namespace MonoGameEngine.Engine.Services;
public interface ICameraServiceFactory
{
    ICameraService Create(GraphicsDeviceManager graphicsDeviceManager, IWorldToPixelConverter worldToPixelConverter);
}
