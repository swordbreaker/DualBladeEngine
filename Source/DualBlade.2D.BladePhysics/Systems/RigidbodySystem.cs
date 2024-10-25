using DualBlade._2D.BladePhysics.Components;
using DualBlade._2D.BladePhysics.Models;
using DualBlade._2D.BladePhysics.Services;
using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using Microsoft.Extensions.DependencyInjection;

namespace DualBlade._2D.BladePhysics.Systems;

public class RigidBodySystem(IGameContext gameContext) : ComponentSystem<RigidBody, TransformComponent>(gameContext)
{
    private readonly PhysicsManager physicsManager = gameContext.ServiceProvider.GetRequiredService<PhysicsManager>();

    protected override void OnAdded(ref IEntity entity, ref RigidBody body, ref TransformComponent transform)
    {
        if (entity.TryGetComponent<ColliderComponent>(out var colliderComponent))
        {
            foreach (var collider in colliderComponent.Colliders)
            {
                physicsManager.Add(collider, transform.Position);
            }
        }
    }

    protected override void OnDestroy(RigidBody body, TransformComponent transform, IEntity entity)
    {
        if (entity.TryGetComponent<ColliderComponent>(out var colliderComponent))
        {
            foreach (var collider in colliderComponent.Colliders)
            {
                physicsManager.Remove(collider, transform.Position);
            }
        }
    }

    protected override void Update(ref RigidBody body, ref TransformComponent transform, ref IEntity entity, GameTime gameTime)
    {
        var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        body.Velocity += body.Acceleration * dt;
        var newPos = transform.Position + body.Velocity * dt;

        if (entity.TryGetComponent<ColliderComponent>(out var collider))
        {
            foreach (var c in collider.Colliders)
            {
                if (physicsManager.GetCollisions(c).FirstOrDefault() is CollisionInfo info)
                {
                    var impulse = CalculateImpulse(body, ref info);
                    body.Velocity += impulse / body.Mass;

                    newPos += info.Normal * info.PenetrationDepth;
                }
            }
        }

        foreach (var c in collider.Colliders)
        {
            physicsManager.Update(c, transform.Position, newPos);
        }

        transform.Position = newPos;
    }

    private Vector2 CalculateImpulse(RigidBody body, ref CollisionInfo info)
    {
        // Calculate impulse based on collision info and object properties
        float restitution = 0.5f; // Coefficient of restitution
        float j = -(1 + restitution) * Vector2.Dot(body.Velocity, info.Normal);
        j /= 1 / body.Mass;
        return j * info.Normal;
    }
}
