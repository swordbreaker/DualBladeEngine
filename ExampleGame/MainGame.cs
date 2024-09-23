using DualBlade.Core;
using DualBlade.Core.Services;
using ExampleGame.Scenes;
using Microsoft.Xna.Framework;

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
