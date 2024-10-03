using DualBlade._2D.Physics.Systems;
using DualBlade._2D.Rendering.Systems;
using DualBlade.Core;
using DualBlade.Core.Components;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using FluidBattle.Scenes;
using FluidBattle.Systems;
using DualBlade.Core.Entities;
using Microsoft.Xna.Framework.Graphics;

namespace FluidBattle;
public class MainGame : BaseGame
{
    public MainGame(IGameCreationContext context) : base(context)
    {
        this.IsMouseVisible = true;
    }

    protected override void InitializeGlobalSystems()
    {
        World.AddSystem<RenderSystem>();
        World.AddSystem<PhysicSystem>();
        World.AddSystem<KinematicSystem>();
        World.AddSystem<InputSystem>();
        World.AddSystem<FpsDisplaySystem>();
        World.AddSystem<DebugColliderSystem>();
    }

    protected override void Initialize()
    {
        base.Initialize();

        var e = new Entity();
        e.AddComponent(new FpsDisplayComponent { Font = GameEngine.Load<SpriteFont>("DefaultFont") });

        World.AddEntities(e);
        this.SceneManager.AddSceneExclusively<MainScene>();
    }
}
