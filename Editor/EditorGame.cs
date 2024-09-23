using DualBlade._2D.Rendering.Systems;
using DualBlade.Core;
using DualBlade.Core.Factories;
using DualBlade.Core.Scenes;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using Editor.Systems;
using ExampleGame.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Editor;
public class EditorGame : BaseGame
{
    public EditorGame(IGameCreationContext context) : base(context)
    {
        IsMouseVisible = true;
    }

    protected override void InitializeGlobalSystems()
    {
        World.AddSystem<RenderSystem>();
        World.AddSystem<CameraSystem>();
        World.AddSystem<InputSystem>();
    }

    protected override void Initialize()
    {
        this.AddEditorScene(new MainScene(this.Context));

        base.Initialize();
    }

    private void AddEditorScene(IGameScene gameScene)
    {
        var root = gameScene.Root;
        World.AddEntity(root);

        //foreach (var system in gameScene.Systems)
        //{
        //    GameWorld.AddSystem(system);
        //}
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        base.Draw(gameTime);
    }
}
