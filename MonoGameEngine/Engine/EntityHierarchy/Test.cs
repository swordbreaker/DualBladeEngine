using MonoGameEngine.Engine.Entities;
using MonoGameEngine.Engine.Worlds;
using System.Collections.Generic;

namespace MonoGameEngine.Engine.EntityHierarchy;

internal class Test : YamlGameScene
{
    public Test(IWorld world) : base(world)
    {

    }
    

    protected override IEnumerable<IEntity> SetupEntities()
    {
        yield return new TransformEntity()
        {
            World = World
        };
    }
}
