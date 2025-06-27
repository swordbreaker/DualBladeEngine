using BulletHell.Desktop.Components;
using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;

namespace BulletHell.Desktop.Systems;

public class ChainMoveSystem(IGameContext gameContext) : ComponentSystem<ChainMoveComponent, TransformComponent>(gameContext)
{
    protected override void OnAdded(ref IEntity entity, ref ChainMoveComponent component, ref TransformComponent component2)
    {
        foreach (var link in component.ChainLinks)
        {
            World.AddEntity(link);
        }
    }

    protected override void Update(ref ChainMoveComponent chain, ref TransformComponent transform, ref IEntity entity, GameTime gameTime)
    {
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (chain.IsChainHead)
        {
            // Head moves normally
            chain.LastPosition = transform.Position;
            transform.Position += chain.Velocity * deltaTime;
        }
        else if (chain.ChainParent != null)
        {
            // Get parent position
            var parent = chain.ChainParent;
            if (parent.TryGetComponent<TransformComponent>(out var parentTransform) &&
                parent.TryGetComponent<ChainMoveComponent>(out var parentChain))
            {
                // Calculate desired position (fixed distance from parent)
                var directionToParent = parentChain.LastPosition - transform.Position;
                var currentDistance = directionToParent.Length();

                if (currentDistance > chain.ChainLinkDistance)
                {
                    // Pull towards parent to maintain chain distance
                    var pullDirection = Vector2.Normalize(directionToParent);
                    var excessDistance = currentDistance - chain.ChainLinkDistance;

                    // Apply spring-like force
                    var pullForce = pullDirection * excessDistance * chain.ChainStiffness;
                    chain.LastPosition = transform.Position;
                    transform.Position += pullForce * deltaTime;
                }
                else
                {
                    // Follow the parent's last position with some lag
                    var followDirection = parentChain.LastPosition - transform.Position;
                    if (followDirection.Length() > 0.1f)
                    {
                        chain.LastPosition = transform.Position;
                        transform.Position = Vector2.Lerp(transform.Position,
                            parentChain.LastPosition - Vector2.Normalize(followDirection) * chain.ChainLinkDistance,
                            chain.ChainStiffness * deltaTime);
                    }
                }
            }
        }
    }
}
