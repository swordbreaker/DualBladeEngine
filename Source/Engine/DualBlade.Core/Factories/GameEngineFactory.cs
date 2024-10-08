﻿using DualBlade.Core.Services;
using Microsoft.Xna.Framework.Content;

namespace DualBlade.Core.Factories;

public sealed class GameEngineFactory(
    IInputManager inputManager,
    IWorldToPixelConverterFactory worldToPixelConverterFactory,
    ICameraServiceFactory cameraServiceFactory) : IGameEngineFactory
{
    public IGameEngine Create(Game game, ContentManager? contentManager = null)
    {
        var graphicsDeviceManager = new GraphicsDeviceManager(game);
        var worldToPixelConverter = worldToPixelConverterFactory.Create(graphicsDeviceManager);
        var cameraService = cameraServiceFactory.Create(graphicsDeviceManager, worldToPixelConverter);

        return new GameEngine(new SpriteFactory(worldToPixelConverter, graphicsDeviceManager))
        {
            Content = contentManager ?? game.Content,
            WorldToPixelConverter = worldToPixelConverter,
            GraphicsDeviceManager = graphicsDeviceManager,
            InputManager = inputManager,
            CameraService = cameraService,
        };
    }
}
