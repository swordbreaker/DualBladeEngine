using AutomaticInterface;
using DualBlade.Core;
using DualBlade.Core.Factories;

namespace DualBlade.Core.Services;

[GenerateAutomaticInterface]
public class GameCreationContext(
    IWorldFactory worldFactory,
    IGameEngineFactory gameEngineFactory,
    IGameContext gameContext,
    IServiceProvider serviceProvider) : IGameCreationContext
{
    public IServiceProvider ServiceProvider => serviceProvider;

    public IGameContext CreateContext(BaseGame game)
    {
        var gameEngine = gameEngineFactory.Create(game);
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
