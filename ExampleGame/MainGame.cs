using ExampleGame.Scenes;
using Microsoft.Xna.Framework;
using MonoGameEngine;
using MonoGameEngine.Engine.Services;
using MonoGameEngine.Engine.Systems;
using MonoGameEngine.Engine.Worlds;
using MonoGameGum.Forms;
using RenderingLibrary;

namespace ExampleGame;

public class MainGame : BaseGame
{
    private readonly IJobQueue _jobQueue;

    public MainGame(IWorldFactory worldFactory, IGameEngineFactory gameEngineFactory, ISceneManagerFactory sceneManagerFactory, IJobQueue jobQueue) : base(worldFactory, gameEngineFactory, sceneManagerFactory)
    {
        Content.RootDirectory = "Content";
        _jobQueue = jobQueue;
    }

    protected override void Initialize()
    {
        base.Initialize();

        this.IsMouseVisible = true;

        SystemManagers.Default = new SystemManagers();
        SystemManagers.Default.Initialize(GameEngine.GraphicsDeviceManager.GraphicsDevice, fullInstantiation: true);
        FormsUtilities.InitializeDefaults();

        SceneManager.AddSceneExclusively(new MainMenuScene(GameWorld, GameEngine, SceneManager, _jobQueue));
    }

    protected override void LoadContent()
    {
        //var data = this.GameEngine.Load<string>("Menu");
        ////string data = File.ReadAllText("Content/Menu.xmmp");
        //var project = Project.LoadFromXml(data);
        //Desktop!.Root = project.Root;
    }

    protected override void Update(GameTime gameTime)
    {
        //SystemManagers.Default.Activity(gameTime.TotalGameTime.TotalSeconds);
        //FormsUtilities.Update(gameTime, Root);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
        //SystemManagers.Default.Draw();
    }
}
