using DualBlade._2D.BladePhysics.Components;
using DualBlade._2D.BladePhysics.Models;
using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using FluidBattle.Components;

namespace FluidBattle.Entities;

[RequiredComponent<TransformComponent>]
[RequiredComponent<FluidComponent>]
[RequiredComponent<ColliderComponent>]
[RequiredComponent<RigidBody>]
public partial struct FluidEntity : IEntity
{
    public FluidEntity(IGameContext context, Vector2 position, float radius, float scale, int player, Color color)
    {
        InitComponents();

        var worldToPixel = context.GameEngine.WorldToPixelConverter;

        var transform = new TransformComponent
        {
            Position = position
        };

        var collider = new CircleCollider(Zero, radius);

        AddComponent(transform);
        AddComponent(new ColliderComponent(collider));
        AddComponent(new FluidComponent { Player = player, Color = color });
        AddComponent(new RigidBody { Mass = 1, CollectCollisionEvents = true });
    }
}