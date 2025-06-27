using BulletHell.Desktop.Components;
using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;

namespace BulletHell.Desktop.Entities;

[AddComponent<DestroyOnScreenBoundsComponent>]
public partial struct RocketBulletEntity : IEntity
{
    public RocketBulletEntity(
        Vector2 position,
        TransformComponent targetTransform,
        Vector2 velocity,
        IGameContext context)
    {
        var sprite = context.GameEngine.CreateSprite("arrows");
        var renderer = new RenderComponent
        {
            Color = Color.Red,
        };
        renderer.SetSprite(sprite);

        AddComponent(renderer);
        AddComponent(new TransformComponent
        {
            Position = position,
            Scale = Vector2.One
        });

        AddComponent(new RocketMoveComponent
        {
            Speed = 20f,
            TargetTransform = targetTransform,
            Velocity = velocity
        });
    }
}
