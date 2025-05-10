using System.Drawing;
using BulletHell.Desktop.Components;
using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Extensions;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;

namespace BulletHell.Desktop.Systems;

public class DestroyOnScreenBoundSystem(IGameContext context) : ComponentSystem<TransformComponent, DestroyOnScreenBoundsComponent>(context)
{
    private const float Threshold = 0.1f;

    private readonly RectangleF Bounds = 
        new(
            -context.GameEngine.GameSize.X / 2 - Threshold,
            -context.GameEngine.GameSize.Y / 2 - Threshold,
            context.GameEngine.GameSize.X + Threshold * 2,
            context.GameEngine.GameSize.Y + Threshold * 2
        )
    ;

    protected override void Update(ref TransformComponent transform, ref DestroyOnScreenBoundsComponent _, ref IEntity entity, GameTime gameTime)
    {
        if(!Bounds.Contains(transform.Position.ToPointF()))
        {
            Ecs.DestroyEntity(entity);
        }
    }
}
