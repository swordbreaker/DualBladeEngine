using BulletHell.Desktop.Components;
using BulletHell.Desktop.Helpers;
using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;

namespace BulletHell.Desktop.Entities;

[AddComponent<DestroyOnScreenBoundsComponent>]
[RequiredComponent<TransformComponent>]
[RequiredComponent<WaveMoveComponent>]
[RequiredComponent<RenderComponent>]
public partial struct WaveBulletEntity : IEntity
{
    public WaveBulletEntity(
        Vector2 position,
        Vector2 direction,
        float speed,
        float amplitude,
        float frequency,
        Color color,
        IGameContext gameContext)
    {
        var spriteFactory = gameContext.GameEngine.SpriteFactory;
        var graphicDevice = gameContext.GameEngine.GraphicsDeviceManager.GraphicsDevice;

        var circleProps = new CircleProperties
        {
            Radius = 8,
            StrokeColor = Color.Black,
            FillColor = color,
        };

        var transform = new TransformComponent
        {
            Position = position
        };

        var waveMove = new WaveMoveComponent
        {
            Direction = Vector2.Normalize(direction),
            Speed = speed,
            Amplitude = amplitude,
            Frequency = frequency,
            Phase = 0,
            PerpendicularDirection = Vector2.Normalize(new Vector2(-direction.Y, direction.X))
        };

        var spriteRender = new RenderComponent();
        spriteRender.SetSprite(spriteFactory.CreateSprite(new CircleTexture(graphicDevice, circleProps)));

        AddComponent(transform);
        AddComponent(waveMove);
        AddComponent(spriteRender);
    }
}
