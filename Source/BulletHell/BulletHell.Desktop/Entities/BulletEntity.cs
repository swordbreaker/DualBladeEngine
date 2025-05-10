using BulletHell.Desktop.Components;
using BulletHell.Desktop.Helpers;
using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;

namespace BulletHell.Desktop.Entities;


[AddComponent<DestroyOnScreenBoundsComponent>]
[RequiredComponent<TransformComponent>]
[RequiredComponent<MoveComponent>]
[RequiredComponent<RenderComponent>]
public partial struct BulletEntity : IEntity
{
    public BulletEntity(Vector2 position, Vector2 velocity, CircleProperties circleProperties, IGameContext gameContext)
    {
        var spriteFactory = gameContext.GameEngine.SpriteFactory;
        var graphicDevice = gameContext.GameEngine.GraphicsDeviceManager.GraphicsDevice;

        var transform = new TransformComponent
        {
            Position = position
        };

        var moveComponent = new MoveComponent
        {
            Velocity = velocity
        };

        var spriteRender = new RenderComponent
        {
            Sprite = spriteFactory.CreateSprite(new CircleTexture(graphicDevice, circleProperties))
        };

        AddComponent(transform);
        AddComponent(moveComponent);
        AddComponent(spriteRender);
    }
}
