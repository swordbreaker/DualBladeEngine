using DualBlade._2D.BladePhysics.Components;
using DualBlade._2D.BladePhysics.Models;
using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using Example.PhysicsTest.Components;

namespace Example.PhysicsTest.Entities;

[RequiredComponent<TransformComponent>]
[RequiredComponent<RenderComponent>]
[RequiredComponent<ColliderComponent>]
[RequiredComponent<RigidBody>]
public partial struct PolySquareEntity : IEntity
{

    public PolySquareEntity(IGameContext context, Vector2 position, Vector2 velocity)
    {
        InitComponents();

        var transform = new TransformComponent { Position = position };

        var sprite = context.GameEngine.CreateSprite("Square");
        var renderer = new RenderComponent();
        renderer.SetSprite(sprite);

        var collider = new ColliderComponent(
            new PolygonCollider([
                new Vector2(-sprite.Width / 2, -sprite.Height / 2),
                new Vector2(sprite.Width / 2, -sprite.Height / 2),
                new Vector2(sprite.Width / 2, sprite.Height / 2),
                new Vector2(-sprite.Width / 2, sprite.Height / 2)
            ]));

        var rigidBody = new RigidBody()
        {
            Velocity = velocity,
            CollectCollisionEvents = true
        };

        AddComponent(transform);
        AddComponent(renderer);
        AddComponent(collider);
        AddComponent(rigidBody);
        AddComponent(new ConstantVelocityComponent { Velocity = velocity });
    }
}