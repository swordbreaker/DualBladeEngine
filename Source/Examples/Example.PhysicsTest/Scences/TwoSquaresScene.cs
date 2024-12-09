using DualBlade.Core.Scenes;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using Example.PhysicsTest.Entities;

namespace Example.PhysicsTest.Scences;
public class TwoSquaresScene(IGameContext context) : GameScene(context)
{
    public override IEnumerable<ISystem> SetupSystems() => Enumerable.Empty<ISystem>();

    protected override IEnumerable<EntityBuilder> SetupEntities()
    {
        yield return CreateEntity(new SquareEntity(GameContext, new Vector2(-2, 0), new Vector2(1, 0)));
        yield return CreateEntity(new SquareEntity(GameContext, new Vector2(2, 0), new Vector2(-1, 0)));
    }
}
