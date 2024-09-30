using DualBlade._2D.Rendering.Systems;
using DualBlade.Core;
using DualBlade.Core.Services;
using Microsoft.Xna.Framework;
using PerformanceTest.Scenes;

namespace PerformanceTest;

public class Game : BaseGame
{
    public Game(IGameCreationContext creationContext) : base(creationContext)
    {
        this.IsFixedTimeStep = false;
        Context.GameEngine.GraphicsDeviceManager.SynchronizeWithVerticalRetrace = false;
    }

    protected override void InitializeGlobalSystems()
    {
        base.InitializeGlobalSystems();
        World.AddSystem<RenderSystem>();
    }

    protected override void Initialize()
    {
        base.Initialize();
        SceneManager.AddScene<MessureUiScene>();
        SceneManager.AddScene<MainScene>();
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}
