using DualBlade._2D.Rendering.Systems;
using DualBlade.Core;
using DualBlade.Core.Services;
using ExampleGame.Scenes;
using Systems;

namespace ExampleGame;

public class MainGame : BaseGame
{
    public MainGame(IGameCreationContext context) : base(context)
    {
        Content.RootDirectory = "Content";
    }

    protected override void Initialize()
    {
        base.Initialize();
        this.IsMouseVisible = true;
        SceneManager.AddSceneExclusively<MainMenuScene>();
    }

    protected override void InitializeGlobalSystems()
    {
        base.InitializeGlobalSystems();
        World.AddSystem<RenderSystem>();
        World.AddSystem<PhysicSystem>();
        World.AddSystem<KinematicSystem>();
    }
}
