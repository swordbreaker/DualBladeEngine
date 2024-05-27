using ExampleGame.Scenes;
using MonoGameEngine;
using MonoGameEngine.Engine.Worlds;
using Myra.Graphics2D.UI;

namespace ExampleGame;

public class MainGame : BaseGame
{
    public MainGame(IWorldFactory worldFactory, IGameEngineFactory gameEngineFactory) : base(worldFactory, gameEngineFactory)
    {
        Content.RootDirectory = "Content";
    }

    protected override void Initialize()
    {
        this.AddSceneExclusively(new MainScene(GameWorld, GameEngine));
        base.Initialize();
    }

    protected override void LoadContent()
    {
        string data = File.ReadAllText("Content/Menu.xmmp");
        var project = Project.LoadFromXml(data);
        Desktop!.Root = project.Root;
    }
}
