using MonoGameEngine.Engine.Entities;
using MonoGameEngine.Engine.Systems;
using MonoGameEngine.Engine.Worlds;
using System.Collections.Generic;

namespace MonoGameEngine.Engine.Scenes;

public abstract class GameScene : IGameScene
{
    protected readonly IWorld World;

    public GameScene(IWorld world)
    {
        World = world;
        Root = new RootEntity
        {
            Childs = SetupEntities(),
            World = world
        };
        Systems = SetupSystems();
    }

    public IEntity Root { get; }

    public IEnumerable<ISystem> Systems { get; }

    protected abstract IEnumerable<IEntity> SetupEntities();
    public abstract IEnumerable<ISystem> SetupSystems();

    public void Dispose()
    {
        World.Destroy(Root);
        World.Destroy(Systems);
    }

    protected TSystem CreateSystem<TSystem>() where TSystem : ISystemWithWorld, new() =>
        new() { World = World };
}