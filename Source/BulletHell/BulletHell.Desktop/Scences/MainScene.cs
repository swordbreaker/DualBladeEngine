using System.Collections.Generic;
using BulletHell.Desktop.Entities;
using BulletHell.Desktop.Services;
using BulletHell.Desktop.Systems;
using DualBlade.Core.Scenes;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;

namespace BulletHell.Desktop.Scences;

public class MainScene(IGameContext context) : GameScene(context)
{
    public override IEnumerable<ISystem> SetupSystems()
    {
        // yield return new TestSystem(GameContext);
        yield return new MusicSystem(GameContext);
        yield return new FrequencyMovementSystem(GameContext);
        yield return new MoveToSystem(GameContext);
        yield return new SpawnerSystem(GameContext);
    }

    protected override IEnumerable<EntityBuilder> SetupEntities()
    {
        // var musicEntity = new MusicEntity("./fighter-269805.mp3");
        // var musicEntity = new MusicEntity("./necrodancer_bonus_Old1-4.mp3");
        var musicEntity = new MusicEntity("./necrodancer_zone_1-2.mp3");

        yield return CreateEntity(musicEntity);
        yield return CreateEntity(new SpawnerEntity(new Vector2(0, 0), GameContext));
    }
}
