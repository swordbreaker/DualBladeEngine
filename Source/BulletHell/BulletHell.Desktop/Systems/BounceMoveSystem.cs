using BulletHell.Desktop.Components;
using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;

namespace BulletHell.Desktop.Systems;

public class BounceMoveSystem(IGameContext context) : ComponentSystem<TransformComponent, BounceMoveComponent>(context)
{
    private Vector2 GameSize => GameContext.GameEngine.GameSize;

    protected override void Update(ref TransformComponent transform, ref BounceMoveComponent bounce, ref IEntity entity, GameTime gameTime)
    {
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        var newPosition = transform.Position + bounce.Velocity * deltaTime;

        // Check for screen bounds collision
        var screenBounds = new Rectangle(
            -(int)(GameSize.X / 2),
            -(int)(GameSize.Y / 2),
            (int)GameSize.X,
            (int)GameSize.Y
        );

        bool bounced = false;

        // Check horizontal bounds
        if (newPosition.X <= screenBounds.Left || newPosition.X >= screenBounds.Right)
        {
            bounce.Velocity.X = -bounce.Velocity.X * bounce.BounceDamping;
            newPosition.X = MathHelper.Clamp(newPosition.X, screenBounds.Left, screenBounds.Right);
            bounced = true;
        }

        // Check vertical bounds
        if (newPosition.Y <= screenBounds.Top || newPosition.Y >= screenBounds.Bottom)
        {
            bounce.Velocity.Y = -bounce.Velocity.Y * bounce.BounceDamping;
            newPosition.Y = MathHelper.Clamp(newPosition.Y, screenBounds.Top, screenBounds.Bottom);
            bounced = true;
        }

        transform.Position = newPosition;

        // Handle bounce count and effects
        if (bounced)
        {
            bounce.BouncesRemaining--;

            if (bounce.FlipOnBounce && entity.TryGetComponent<RenderComponent>(out var renderComponent))
            {
                // Change color or flip sprite on bounce
                renderComponent.Color = Color.Lerp(renderComponent.Color, Color.Yellow, 0.5f);
                entity.UpdateComponent(renderComponent);
            }

            // Destroy if no bounces remaining
            if (bounce.BouncesRemaining <= 0)
            {
                Ecs.DestroyEntity(entity);
                return;
            }
        }

        // Update rotation to face movement direction
        if (bounce.Velocity.Length() > 0.1f)
        {
            transform.Rotation = MathHelper.ToDegrees(MathF.Atan2(bounce.Velocity.Y, bounce.Velocity.X));
        }
    }
}
