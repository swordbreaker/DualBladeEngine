using Microsoft.Xna.Framework;
using MonoGameEngine;
using MonoGameEngine.Engine.Entities;
using MonoGameEngine.Engine.Extensions;
using MonoGameEngine.Engine.Worlds;
using Color = Microsoft.Xna.Framework.Color;

namespace Editor;
public class MainGame : Game
{
    protected override void Initialize()
    {
        base.Initialize();

        //var (w, h) = GameEngine.GameSize;

        //var a = new SpriteEntity() { World = GameWorld };
        //var b = new SpriteEntity() { World = GameWorld };
        //var c = new SpriteEntity() { World = GameWorld };

        //a.AddChild(b);

        //GameWorld.AddEntity(a);
        //GameWorld.AddEntity(c);
    }

    protected override void Draw(GameTime gameTime)
    {
        this.GraphicsDevice.Clear(Color.CornflowerBlue);
    }
}
