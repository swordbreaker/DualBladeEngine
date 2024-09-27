using DualBlade._2D.Rendering.Systems;
using DualBlade.Core;
using DualBlade.Core.Services;
using PerformanceTest.Scenes;

namespace PerformanceTest;
public class Game(IGameCreationContext creationContext) : BaseGame(creationContext)
{
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
}
