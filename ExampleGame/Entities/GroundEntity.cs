using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameEngine.Engine.Entities;
using MonoGameEngine.Engine.Worlds;
using nkast.Aether.Physics2D.Dynamics;

namespace ExampleGame.Entities;

public class GroundEntity : SpriteEntity
{
    public readonly Body PhysicsBody;

    public GroundEntity(IGameEngine gameEngine)
    {
        var (w, h) = gameEngine.GameSize;
        var size = new Vector2(w, 10);
        var pos = new Vector2(w / 2, h - size.Y);

        Renderer.SetTexture(gameEngine.Load<Texture2D>("pixel"));
        Renderer.Color = Color.Green;
        Transform.Scale = size;
        Transform.Position = pos;

        PhysicsBody = gameEngine.PhysicsManager.CreateBody(pos, bodyType: BodyType.Static);
        PhysicsBody.Tag = this;
        var fixture = PhysicsBody.CreateRectangle(size.X, size.Y, 1f, Vector2.Zero);
        fixture.Tag = this;
        fixture.Restitution = 0.3f;
        fixture.Friction = 0.5f;
    }

    public Vector2 Position
    {
        get => Transform.Position;
        set
        {
            PhysicsBody.Position = value;
            Transform.Position = value;
        }
    }

    public Vector2 Size
    {
        get => Transform.Scale;
        set
        {
            Transform.Scale = value;
            PhysicsBody.Remove(PhysicsBody.FixtureList[0]);
            PhysicsBody.CreateRectangle(value.X, value.Y, 1f, Vector2.Zero);
        }
    }

    public Color Color
    {
        get => Renderer.Color;
        set => Renderer.Color = value;
    }
}
