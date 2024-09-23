using AutomaticInterface;

namespace DualBlade.Core.Services;

[GenerateAutomaticInterface]
public class SceneManagerFactory(IServiceProvider serviceProvider) : ISceneManagerFactory
{
    public ISceneManager CreateSceneManager(IGameContext gameContext) =>
        new SceneManager(gameContext, serviceProvider);
}
