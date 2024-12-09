using DualBlade._2D.BladePhysics.Components;
using DualBlade._2D.BladePhysics.Models;
using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.PhysicsTest.Entities;

[RequiredComponent<TransformComponent>]
[RequiredComponent<RenderComponent>]
[RequiredComponent<ColliderComponent>]
[RequiredComponent<RigidBody>]
public partial struct SquareEntity : IEntity
{
    public SquareEntity(IGameContext context, Vector2 position, Vector2 velocity)
    {
        InitComponents();

        var transform = new TransformComponent { Position = position };

        var sprite = context.GameEngine.CreateSprite("Square");
        var renderer = new RenderComponent();
        renderer.SetSprite(sprite);

        var collider = new ColliderComponent(new RectangleCollider(Vector2.Zero, new Vector2(sprite.Width, sprite.Height)));

        var rigidBody = new RigidBody()
        {
            Velocity = velocity,
            CollectCollisionEvents = true
        };

        AddComponent(transform);
        AddComponent(renderer);
        AddComponent(collider);
        AddComponent(rigidBody);
    }
}
