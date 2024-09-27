using DualBlade._2D.Physics.Systems;
using DualBlade._2D.Rendering.Systems;
using DualBlade.Core;
using DualBlade.Core.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using ExampleGame.Scenes;
using Microsoft.Xna.Framework.Graphics;

namespace ExampleGame;

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

        SceneManager.AddSceneExclusively<MainMenuScene>();
    }

    protected override void InitializeGlobalSystems()
    {
        base.InitializeGlobalSystems();
        World.AddSystem<RenderSystem>();
        World.AddSystem<PhysicSystem>();
        World.AddSystem<KinematicSystem>();
        World.AddSystem<FpsDisplaySystem>();
    }
}
