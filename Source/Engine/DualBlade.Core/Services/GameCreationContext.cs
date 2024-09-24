using AutomaticInterface;
using DualBlade.Core.Factories;
using Microsoft.Xna.Framework.Content;

namespace DualBlade.Core.Services;

[GenerateAutomaticInterface]
public sealed class GameCreationContext(
    IWorldFactory worldFactory,
    IGameEngineFactory gameEngineFactory,
    IGameContext gameContext,
    IServiceProvider serviceProvider) : IGameCreationContext
{
    public IServiceProvider ServiceProvider => serviceProvider;

    public IGameContext CreateContext(BaseGame game, ContentManager? contentManager = null)
    {
        var gameEngine = gameEngineFactory.Create(game, contentManager);
        var gameWorld = worldFactory.Create(gameEngine);

        if (gameContext is IGameContextInitializer gameContextInitializer)
        {
            gameContextInitializer.Init(gameEngine, game, gameWorld);
        }
        else
        {
            throw new Exception("GameFacade must implement IGameFacadeInitializer");
        }

        return gameContext;
    }
}
