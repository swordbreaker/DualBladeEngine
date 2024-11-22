using DualBlade._2D.BladePhysics.Components;
using DualBlade._2D.BladePhysics.Services;
using DualBlade._2D.Rendering.Components;
using DualBlade._2D.Rendering.Extensions;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using Microsoft.Extensions.DependencyInjection;

namespace DualBlade._2D.BladePhysics.Systems;

public class ColliderSystem(IGameContext gameContext) : ComponentSystem<ColliderComponent>(gameContext)
{
    private readonly IPhysicsManager
        _physicsManager = gameContext.ServiceProvider.GetRequiredService<IPhysicsManager>();

    protected override void OnAdded(ref IEntity entity, ref ColliderComponent component)
    {
        if (!entity.TryGetComponent<ColliderComponent>(out var colliderComponent)) return;

        foreach (var collider in colliderComponent.Colliders)
        {
            collider.Offset = Ecs.AbsolutePosition(entity);
            collider.Scale = Ecs.AbsoluteScale(entity);

            _physicsManager.Add(collider);
        }
    }

    protected override void OnDestroy(ColliderComponent component, IEntity entity)
    {
        if (entity.TryGetComponent<ColliderComponent>(out var colliderComponent))
        {
            foreach (var collider in colliderComponent.Colliders)
            {
                _physicsManager.Remove(collider);
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
            collider.Offset = Ecs.AbsolutePosition(entity);
            collider.Scale = Ecs.AbsoluteScale(entity);
        }
    }
}