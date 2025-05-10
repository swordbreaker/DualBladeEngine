using BulletHell.Desktop.Scences;
using BulletHell.Desktop.Systems;
using DualBlade._2D.Rendering.Systems;
using DualBlade.Core;
using DualBlade.Core.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using Microsoft.Xna.Framework.Graphics;

namespace BulletHell.Desktop;

public class MainGame : BaseGame
{
    public MainGame(IGameCreationContext context) : base(context)
    {
        this.IsMouseVisible = true;
        this.IsFixedTimeStep = false;
        Context.GameEngine.GraphicsDeviceManager.SynchronizeWithVerticalRetrace = false;
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
    }
}
