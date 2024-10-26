using DualBlade._2D.BladePhysics.Components;
using DualBlade._2D.BladePhysics.Services;
using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using Microsoft.Extensions.DependencyInjection;

namespace DualBlade._2D.BladePhysics.Systems;

public class ColliderSystem(IGameContext gameContext) : ComponentSystem<ColliderComponent>(gameContext)
{
    private readonly PhysicsManager physicsManager = gameContext.ServiceProvider.GetRequiredService<PhysicsManager>();

    protected override void OnAdded(ref IEntity entity, ref ColliderComponent component)
    {
        TransformComponent transform = default;
        if (entity.TryGetComponent<TransformComponent>(out var transformComponent))
        {
            transform = transformComponent;
        }

        if (entity.TryGetComponent<ColliderComponent>(out var colliderComponent))
        {
            foreach (var collider in colliderComponent.Colliders)
            {
                collider.Offset = transform.Position;
                collider.Scale = transform.Scale;

                physicsManager.Add(collider);
            }
        }
    }

    protected override void OnDestroy(ColliderComponent component, IEntity entity)
    {
        if (entity.TryGetComponent<ColliderComponent>(out var colliderComponent))
        {
            foreach (var collider in colliderComponent.Colliders)
            {
                physicsManager.Remove(collider);
            }
        }
    }

    protected override void Update(ref ColliderComponent component, ref IEntity entity, GameTime gameTime)
    {
        if (!entity.TryGetComponent<TransformComponent>(out var transform))
        {
            return;
        }

        foreach (var collider in component.Colliders)
        {
            collider.Offset = transform.Position;
            collider.Scale = transform.Scale;
        }
    }
}
