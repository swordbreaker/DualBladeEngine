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
        // Core music and audio analysis
        yield return new MusicSystem(GameContext);
        yield return new FrequencyMovementSystem(GameContext);
        yield return new MoveToSystem(GameContext);
        yield return new PlayerSystem(GameContext);

        // Basic movement systems
        yield return new MoveSystem(GameContext);
        yield return new RocketMoveSystem(GameContext);

        // Advanced movement systems
        yield return new WaveMoveSystem(GameContext);
        yield return new SpiralMoveSystem(GameContext);
        yield return new HomingMoveSystem(GameContext);
        yield return new BounceMoveSystem(GameContext);
        yield return new AccelerationMoveSystem(GameContext);
        yield return new OrbitMoveSystem(GameContext);
        yield return new ClusterBulletSystem(GameContext);

        // New advanced movement systems
        yield return new ZigzagMoveSystem(GameContext);
        yield return new PulsingMoveSystem(GameContext);
        yield return new GravitationalMoveSystem(GameContext);
        yield return new SeekingMoveSystem(GameContext);
        yield return new ChainMoveSystem(GameContext);

        // Spawning systems
        // yield return new SpawnerSystem(GameContext);sa
        yield return new AudioDrivenSpawnerSystem(GameContext);
    }

    protected override IEnumerable<EntityBuilder> SetupEntities()
    {
        var musicEntity = new MusicEntity("./fighter-269805.mp3");
        // var musicEntity = new MusicEntity("./necrodancer_bonus_Old1-4.mp3");
        // var musicEntity = new MusicEntity("./necrodancer_zone_1-2.mp3");

        var player = new PlayerEntity(new Vector2(0, -2), GameContext);

        yield return CreateEntity(musicEntity);
        yield return CreateEntity(new SpawnerEntity(new Vector2(0, 0), GameContext));
        yield return CreateEntity(player);
    }
}
