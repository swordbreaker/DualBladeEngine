using MonoGameEngine.Engine.Services;
using MonoGameEngine.Engine.Systems;
using MonoGameEngine.Engine.Worlds;
using Myra;
using Myra.Graphics2D.UI;
using Color = Microsoft.Xna.Framework.Color;

namespace MonoGameEngine;

public abstract class BaseGame : Game
{
    protected IGameEngine GameEngine;
    protected IWorld GameWorld;
    protected Desktop? Desktop;
    protected Color BackgroundColor = Color.CornflowerBlue;
    protected ISceneManager SceneManager;

    public BaseGame(IWorldFactory worldFactory, IGameEngineFactory gameEngineFactory, ISceneManagerFactory sceneManagerFactory)
    {
        GameEngine = gameEngineFactory.Create(this);
        GameWorld = worldFactory.Create(GameEngine);
        Content.RootDirectory = "Content";
        SceneManager = sceneManagerFactory.CreateSceneManager(GameWorld);
    }

    protected override void Initialize()
    {
        GameEngine.Initialize();
        InitializeGlobalSystems();
        MyraEnvironment.Game = this;
        Desktop = new Desktop();

        GameWorld.Initialize();
        base.Initialize();
    }

    protected virtual void InitializeGlobalSystems()
    {
        GameWorld.AddSystem<RenderSystem>();
        GameWorld.AddSystem<KinematicSystem>();
        GameWorld.AddSystems(new InputSystem());
    }

    protected override void Update(GameTime gameTime)
    {
        GameWorld.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(BackgroundColor);
        GameWorld.Draw(gameTime);

        Desktop!.Render();
        base.Draw(gameTime);
    }
}
