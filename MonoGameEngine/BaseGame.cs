using Microsoft.Xna.Framework;
using MonoGameEngine.Engine.Entities;
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

    protected void AddEntity<TEntity>(TEntity entity) where TEntity : Entity
    {
        GameWorld.AddEntity(entity);
    }

}
