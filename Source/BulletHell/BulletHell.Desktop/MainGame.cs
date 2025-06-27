using BulletHell.Desktop.Scences;
using BulletHell.Desktop.Systems;
using DualBlade._2D.Rendering.Systems;
using DualBlade.Core;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using Microsoft.Xna.Framework.Graphics;

namespace BulletHell.Desktop;

public class MainGame : BaseGame
{
    public MainGame(IGameCreationContext context) : base(context)
    {
        this.IsMouseVisible = false;
        this.IsFixedTimeStep = false;
        Context.GameEngine.GraphicsDeviceManager.SynchronizeWithVerticalRetrace = false;
        Context.GameEngine.GraphicsDeviceManager.PreferredBackBufferWidth = 1280;
        Context.GameEngine.GraphicsDeviceManager.PreferredBackBufferHeight = 720;
        Context.GameEngine.GraphicsDeviceManager.ApplyChanges();
    }

    protected override void Initialize()
    {
        base.Initialize();

        var fpsEntity = new Entity();
        fpsEntity.AddComponent(new FpsDisplayComponent()
        {
            Font = Content.Load<SpriteFont>("DefaultFont"),
        });
        World.AddEntity(fpsEntity);

        SceneManager.AddSceneExclusively<MainScene>();
    }

    protected override void InitializeGlobalSystems()
    {
        base.InitializeGlobalSystems();
        World.AddSystem<FpsDisplaySystem>();
        World.AddSystem<RenderSystem>();
        World.AddSystem<MoveSystem>();
        World.AddSystem<DestroyOnScreenBoundSystem>();
        
        // Add all the movement systems
        // World.AddSystem<WaveMoveSystem>();
        // World.AddSystem<SpiralMoveSystem>();
        // World.AddSystem<HomingMoveSystem>();
        // World.AddSystem<BounceMoveSystem>();
        // World.AddSystem<AccelerationMoveSystem>();
        // World.AddSystem<OrbitMoveSystem>();
        // World.AddSystem<ClusterBulletSystem>();
        
        // Add new advanced movement systems
        // World.AddSystem<ZigzagMoveSystem>();
        // World.AddSystem<PulsingMoveSystem>();
        // World.AddSystem<GravitationalMoveSystem>();
        // World.AddSystem<SeekingMoveSystem>();
        // World.AddSystem<ChainMoveSystem>();
    }
}
