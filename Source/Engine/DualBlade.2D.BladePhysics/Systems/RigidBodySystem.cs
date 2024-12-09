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
    private readonly IPhysicsManager physicsManager = gameContext.ServiceProvider.GetRequiredService<IPhysicsManager>();

    protected override void OnAdded(ref IEntity entity, ref RigidBody body, ref TransformComponent transform)
    {
        if (!entity.TryGetComponent<ColliderComponent>(out var colliderComponent)) return;

        foreach (var collider in colliderComponent.Colliders)
        {
            physicsManager.Add(collider);
        }
    }

    protected override void OnDestroy(RigidBody body, TransformComponent transform, IEntity entity)
    {
        if (!entity.TryGetComponent<ColliderComponent>(out var colliderComponent)) return;

        foreach (var collider in colliderComponent.Colliders)
        {
            physicsManager.Remove(collider);
        }
    }

    protected override void FixedUpdate(ref RigidBody body, ref TransformComponent transform, ref IEntity entity,
        GameTime gameTime)
    {
        var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        body.Velocity += body.Acceleration * dt;
        var newPos = transform.Position + body.Velocity * dt;

        if (entity.TryGetComponent<ColliderComponent>(out var collider))
        {
            var collisions = GetCollisions(collider).ToArray();

            foreach (var info in collisions)
            {
                var impulse = CalculateImpulse(body, info);
                body.Velocity += impulse / body.Mass;

                newPos += info.Normal * info.PenetrationDepth;
            }

            physicsManager.SetCollisions(body, collisions);

            foreach (var c in collider.Colliders)
            {
                physicsManager.Update(c, transform.Position, newPos);
            }
        }

        transform.Position = newPos;
    }

    private IEnumerable<CollisionInfo> GetCollisions(ColliderComponent collider)
    {
        foreach (var c in collider.Colliders)
        {
            if (physicsManager.CalculateCollisions(c).FirstOrDefault() is { Collider: not null } info)
            {
                yield return info;
            }
        }
    }

    private static Vector2 CalculateImpulse(RigidBody body, CollisionInfo info)
    {
        // Calculate impulse based on collision info and object properties
        const float restitution = 0.5f; // Coefficient of restitution
        var j = -(1 + restitution) * Vector2.Dot(body.Velocity, info.Normal);
        j /= 1 / (body.Mass + 0.000001f);
        return j * info.Normal;
    }
}