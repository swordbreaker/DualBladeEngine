using Microsoft.Extensions.DependencyInjection;
using MonoGameEngine.Engine.Physics;
using MonoGameEngine.Engine.Rendering;
using MonoGameEngine.Engine.Services;
using System;

namespace MonoGameEngine.Engine.Worlds;

public class GameEngineFactory(
    IServiceProvider _serviceProvider,
    ICameraServiceFactory _cameraServiceFactory,
    IWorldToPixelConverterFactory worldToPixelConverterFactory) : IGameEngineFactory
{
    public IGameEngine Create(Game game)
    {
        var graphicsDeviceManager = new GraphicsDeviceManager(game);
        var worldToPixelConverter = worldToPixelConverterFactory.Create(graphicsDeviceManager);
        var cameraService = _cameraServiceFactory.Create(graphicsDeviceManager, worldToPixelConverter);

        return new GameEngine(new SpriteFactory(worldToPixelConverter))
        {
            Content = game.Content,
            WorldToPixelConverter = worldToPixelConverter,
            GraphicsDeviceManager = graphicsDeviceManager,
            InputManager = _serviceProvider.GetRequiredService<InputManager>(),
            PhysicsManager = _serviceProvider.GetRequiredService<IPhysicsManager>(),
            CameraService = cameraService,
        };
    }
}
