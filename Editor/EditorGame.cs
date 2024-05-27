using Editor.Systems;
using ExampleGame.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameEngine;
using MonoGameEngine.Engine.Scenes;
using MonoGameEngine.Engine.Systems;
using MonoGameEngine.Engine.Worlds;

namespace Editor;
public class EditorGame : BaseGame
{
    public EditorGame(IWorldFactory worldFactory, IGameEngineFactory gameEngineFactory) : base(worldFactory, gameEngineFactory)
    {
        IsMouseVisible = true;
    }

    protected override void InitializeGlobalSystems()
    {
        GameWorld.AddSystem<RenderSystem>();
        GameWorld.AddSystem<CameraSystem>();
        GameWorld.AddSystem(new InputSystem());
    }

    protected override void Initialize()
    {
        this.AddEditorScene(new MainScene(GameWorld, GameEngine));

        base.Initialize();
    }

    private void AddEditorScene(IGameScene gameScene)
    {
        var root = gameScene.Root;
        GameWorld.AddEntity(root);

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
