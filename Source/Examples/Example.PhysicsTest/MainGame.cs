using DualBlade._2D.Rendering.Systems;
using DualBlade.Core;
using DualBlade.Core.Components;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using DualBlade.Core.Entities;
using Microsoft.Xna.Framework.Graphics;
using DualBlade.MyraUi.Systems;
using DualBlade._2D.BladePhysics.Systems;
using Example.PhysicsTest.Scences;
using Example.PhysicsTest.Systems;

namespace Example.PhysicsTest;

public class MainGame : BaseGame
{
    public MainGame(IGameCreationContext context) : base(context)
    {
        IsMouseVisible = false;
    }

    protected override void InitializeGlobalSystems()
    {
        World.AddSystem(new SceneSwitchSystem(this.Context));
        World.AddSystem<RenderSystem>();
        World.AddSystem<InputSystem>();
        World.AddSystem<FpsDisplaySystem>();
        World.AddSystem<MyraDesktopSystem>();
        World.AddSystem<ColliderSystem>();
        World.AddSystem<RigidBodySystem>();
        World.AddSystem<DebugColliderSystem>();
    }

    protected override void Initialize()
    {
        base.Initialize();

        var e = new Entity();
        e.AddComponent(new FpsDisplayComponent { Font = GameEngine.Load<SpriteFont>("DefaultFont") });

        World.AddEntities(e);

        SceneManager.AddSceneExclusively<TwoCirclesScene>();
        // this.SceneManager.AddSceneExclusively<MainMenuScene>();
        //this.SceneManager.AddSceneExclusively<MainScene>();
    }
}