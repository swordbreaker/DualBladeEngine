namespace MonoGameEngine.Engine.Services;
public class CameraServiceFactory : ICameraServiceFactory
{
    public ICameraService Create(GraphicsDeviceManager graphicsDeviceManager, IWorldToPixelConverter worldToPixelConverter) =>
        new CameraService(graphicsDeviceManager, worldToPixelConverter);
}