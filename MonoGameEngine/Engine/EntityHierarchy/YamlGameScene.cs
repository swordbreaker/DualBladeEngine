using MonoGameEngine.Engine.Scenes;
using MonoGameEngine.Engine.Systems;
using MonoGameEngine.Engine.Worlds;
using System.Collections.Generic;

namespace MonoGameEngine.Engine.EntityHierarchy;

public abstract class YamlGameScene : GameScene
{
    protected YamlGameScene(IWorld world) : base(world)
    {
    }

    public override IEnumerable<ISystem> SetupSystems() => [];
}
