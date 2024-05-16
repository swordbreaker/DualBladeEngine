using Microsoft.Xna.Framework;
using Myra;
using Myra.Graphics2D.UI;
using TestMonoGamesProject.Engine.Systems;
using TestMonoGamesProject.Engine.World;
using TestMonoGamesProject.Engine.Worlds;
using Color = Microsoft.Xna.Framework.Color;

namespace TestMonoGamesProject;

public abstract class BaseGame : Game
{
    protected IGameEngine GameEngine;
    protected IWorld GameWorld;
    protected Desktop Desktop;
    protected Color BackgroundColor = Color.CornflowerBlue;

    public BaseGame(WorldFactory worldFactory, IGameEngineFactory gameEngineFactory)
    {
        GameEngine = gameEngineFactory.Create(this);
        GameWorld = worldFactory.Create(GameEngine);
        Content.RootDirectory = "Content";
    }

    protected override void Initialize()
    {
        GameEngine.Initialize();
        InitializeSystems();

        MyraEnvironment.Game = this;
        Desktop = new Desktop();
        base.Initialize();
    }

    protected virtual void InitializeSystems()
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

        Desktop.Render();
        base.Draw(gameTime);
    }
}
