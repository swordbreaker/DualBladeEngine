using AutomaticInterface;
using MonoGameEngine.Engine.Worlds;

namespace MonoGameEngine.Engine.Services;

[GenerateAutomaticInterface]
public class SceneManagerFactory : ISceneManagerFactory
{
    public ISceneManager CreateSceneManager(IWorld gameWorld) =>
        new SceneManager(gameWorld);
}
