using BulletHell.Desktop.Components;
using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Extensions;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;

namespace BulletHell.Desktop.Systems;

public class MoveToSystem(IGameContext context) : ComponentSystem<TransformComponent, MoveToComponent>(context)
{
    protected override void Update(ref TransformComponent transform, ref MoveToComponent moveTo, ref IEntity entity, GameTime gameTime)
    {
        var deltaTime = gameTime.DeltaSeconds();

        if(deltaTime <= 0)
            return;

        // Calculate the direction to the target position
        var direction = moveTo.TargetPosition - transform.Position;
        var distance = direction.Length();

        // If the entity is close enough to the target position, stop moving
        if (distance < moveTo.Speed * deltaTime)
        {
            transform.Position = moveTo.TargetPosition;
            // entity.RemoveComponent<MoveToComponent>();
            return;
        }

        // Normalize the direction and move towards the target position
        direction.Normalize();
        transform.Position += direction * moveTo.Speed * deltaTime;
    }
}
