using DualBlade._2D.Physics.Services;
using DualBlade._2D.Rendering.Entities;
using DualBlade.Core.Services;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;

namespace ExampleGame.Entities;

public class GroundEntity : SpriteEntity
{
    public readonly Body PhysicsBody;

    public GroundEntity(IGameContext context, IPhysicsManager physicsManager)
    {
        var gameEngine = context.GameEngine;

        var (w, h) = gameEngine.GameSize;
        var size = new Vector2(w, 1);
        var pos = new Vector2(0, -h / 2);

        Renderer.SetSprite(gameEngine.CreateSprite("pixel"));
        Renderer.Color = Color.Green;
        Transform.Scale = size / Renderer.Sprite.Size;
        Transform.Position = pos;

        PhysicsBody = physicsManager.CreateBody(pos, bodyType: BodyType.Static);
        PhysicsBody.Tag = this;
        var fixture = PhysicsBody.CreateRectangle(size.X, size.Y, 1f, Vector2.Zero);
        fixture.Tag = this;
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
            PhysicsBody.CreateRectangle(value.X * Renderer.Sprite.Width, value.Y * Renderer.Sprite.Height, 1f, Vector2.Zero);
        }
    }

    public Color Color
    {
        get => Renderer.Color;
        set => Renderer.Color = value;
    }
}
