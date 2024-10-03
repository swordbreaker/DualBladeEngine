using DualBlade._2D.Physics.Services;
using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;

namespace ExampleGame.Entities;

[AddComponent<RenderComponent>]
[AddComponent<TransformComponent>]
public partial struct GroundEntity : IEntity
{
    public readonly Body PhysicsBody;

    public GroundEntity(IGameContext context, IPhysicsManager physicsManager)
    {
        InitComponents();
        using var renderer = RenderComponentProxy;
        using var transform = TransformComponentProxy;

        var gameEngine = context.GameEngine;

        var (w, h) = gameEngine.GameSize;
        var size = new Vector2(w, 1);
        var pos = new Vector2(0, -h / 2);

        renderer.Value.SetSprite(gameEngine.CreateSprite("pixel"));
        renderer.Value.Color = Color.Green;
        transform.Value.Scale = size / renderer.Value.Sprite.Size;
        transform.Value.Position = pos;

        PhysicsBody = physicsManager.CreateBody(pos, bodyType: BodyType.Static);
        PhysicsBody.Tag = this;
        var fixture = PhysicsBody.CreateRectangle(size.X, size.Y, 1f, Vector2.Zero);
        fixture.Tag = this;
    }

    public readonly Vector2 Position
    {
        get => TransformComponentProxy.Value.Position;
        set
        {
            using var transform = TransformComponentProxy;
            PhysicsBody.Position = value;
            transform.Value.Position = value;
        }
    }

    public readonly Vector2 Size
    {
        get => TransformComponentProxy.Value.Scale;
        set
        {
            using var transform = TransformComponentProxy;
            var renderer = RenderComponentProxy.Value;
            transform.Value.Scale = value;
            PhysicsBody.Remove(PhysicsBody.FixtureList[0]);
            PhysicsBody.CreateRectangle(value.X * renderer.Sprite.Width, value.Y * renderer.Sprite.Height, 1f, Vector2.Zero);
        }
    }

    public readonly Color Color
    {
        get => RenderComponentProxy.Value.Color;
        set
        {
            using var renderer = this.RenderComponentProxy;
            renderer.Value.Color = value;
        }
    }
}
