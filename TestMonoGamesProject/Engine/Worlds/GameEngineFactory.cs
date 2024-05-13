using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using System;
using TestMonoGamesProject.Engine.Physics;

namespace TestMonoGamesProject.Engine.Worlds
{
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
            };
        }
    }
}
