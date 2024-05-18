using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using MonoGameEngine.Engine.Physics;
using MonoGameEngine.Engine.Services;
using System;

namespace MonoGameEngine.Engine.Worlds;

public class GameEngineFactory(IServiceProvider _serviceProvider) : IGameEngineFactory
{
    public IGameEngine Create(Game game)
    {
        var graphicsDeviceManager = new GraphicsDeviceManager(game);
        return new GameEngine
        {
            Content = game.Content,
            GraphicsDeviceManager = graphicsDeviceManager,
            InputManager = _serviceProvider.GetRequiredService<InputManager>(),
            PhysicsManager = _serviceProvider.GetRequiredService<IPhysicsManager>(),
            CameraService = new CameraService(graphicsDeviceManager),
        };
    }
}
