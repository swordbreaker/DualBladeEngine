using DualBlade._2D.BladePhysics.Components;
using DualBlade._2D.BladePhysics.Models;
using DualBlade._2D.BladePhysics.Services;
using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MonoGame.Extended.Tweening;

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

    protected override void Update(ref RigidBody body, ref TransformComponent transform, ref IEntity entity, GameTime gameTime)
    {
        var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        body.Velocity += body.Acceleration * dt;
        var newPos = transform.Position + body.Velocity * dt;

        if (entity.TryGetComponent<ColliderComponent>(out var collider))
        {
            foreach (var c in collider.Colliders)
            {
                physicsManager.Update(c, transform.Position, newPos);
            }
        }

        transform.Position = newPos + body.CorrectionVector;
    }

    protected override void FixedUpdate(ref RigidBody body, ref TransformComponent transform, ref IEntity entity,
        GameTime gameTime)
    {
        //var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        //body.Velocity += body.Acceleration * dt;
        //var newPos = transform.Position + body.Velocity * dt;

        if (entity.TryGetComponent<ColliderComponent>(out var collider))
        {
            var collisions = GetCollisions(collider).ToArray();

            var correctionVector = Vector2.Zero;

            foreach (var info in collisions)
            {
                var impulse = CalculateImpulse(body, info);
                body.Velocity += impulse / body.Mass;

                correctionVector += info.Normal * info.PenetrationDepth;
            }

            body.CorrectionVector = correctionVector;
            physicsManager.SetCollisions(body, collisions);
        }

        //transform.Position = newPos;
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
        // The coefficient of restitution ranges from 0 to 1:
        // A value of 1 represents a perfectly elastic collision, where objects rebound with the same relative speed but in opposite directions.
        // A value of 0 indicates a perfectly inelastic collision, where objects do not rebound and end up touching.
        // Most real-world collisions fall between 0 and 1, indicating partial conservation of kinetic energy.
        const float restitution = 0.5f; // Coefficient of restitution
        var j = -(1 + restitution) * Vector2.Dot(body.Velocity, info.Normal);
        j /= 1 / (body.Mass + 0.000001f);
        return j * info.Normal;
    }
}