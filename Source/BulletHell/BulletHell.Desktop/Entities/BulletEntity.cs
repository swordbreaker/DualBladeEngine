using BulletHell.Desktop.Components;
using BulletHell.Desktop.Helpers;
using BulletHell.Desktop.Models;
using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;

namespace BulletHell.Desktop.Entities;


[AddComponent<DestroyOnScreenBoundsComponent>]
// [RequiredComponent<FrequencyMovementComponent>]
[RequiredComponent<TransformComponent>]
[RequiredComponent<MoveComponent>]
[RequiredComponent<RenderComponent>]
public partial struct BulletEntity : IEntity
{
    public BulletEntity(
        SpectogramPoint spectogramPoint,
        Vector2 velocity,
        CircleProperties circleProperties,
        IGameContext gameContext)
    {
        var spriteFactory = gameContext.GameEngine.SpriteFactory;
        var graphicDevice = gameContext.GameEngine.GraphicsDeviceManager.GraphicsDevice;

        var transform = new TransformComponent
        {
            Position = spectogramPoint.Point
        };

        var moveComponent = new MoveComponent
        {
            Velocity = velocity
        };

        var spriteRender = new RenderComponent
        {
            Sprite = spriteFactory.CreateSprite(new CircleTexture(graphicDevice, circleProperties))
        };

        // var frequencyMovement = new FrequencyMovementComponent
        // {
        //     Frequency = spectogramPoint.Frequency,
        //     Decibel = spectogramPoint.Decibel,
        // };

        AddComponent(transform);
        AddComponent(moveComponent);
        AddComponent(spriteRender);
        // AddComponent(frequencyMovement);
    }
}
