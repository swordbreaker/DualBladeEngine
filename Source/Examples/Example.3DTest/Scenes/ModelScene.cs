using System.Collections.Generic;
using DualBlade.Core.Scenes;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;

namespace Example.PhysicsTest.Scenes;

public class ModelScene : GameScene
{
    public ModelScene(IGameContext context) : base(context)
    {
    }

    protected override IEnumerable<EntityBuilder> SetupEntities()
    {
        
    }

    public override IEnumerable<ISystem> SetupSystems()
    {
        throw new System.NotImplementedException();
    }
}