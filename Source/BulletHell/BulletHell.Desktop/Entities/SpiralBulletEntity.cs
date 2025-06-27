using BulletHell.Desktop.Components;
using BulletHell.Desktop.Helpers;
using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;

namespace BulletHell.Desktop.Entities;

[AddComponent<DestroyOnScreenBoundsComponent>]
[RequiredComponent<TransformComponent>]
[RequiredComponent<SpiralMoveComponent>]
[RequiredComponent<RenderComponent>]
public partial struct SpiralBulletEntity : IEntity
{
    public SpiralBulletEntity(
        Vector2 position,
        Vector2 initialVelocity,
        float angularVelocity,
        float radiusGrowth,
        Color color,
        IGameContext gameContext)
    {
        var spriteFactory = gameContext.GameEngine.SpriteFactory;
        var graphicDevice = gameContext.GameEngine.GraphicsDeviceManager.GraphicsDevice;

        var circleProps = new CircleProperties
        {
            Radius = 6,
            StrokeColor = Color.White,
            FillColor = color,
        };

        var transform = new TransformComponent
        {
            Position = position
        };

        var spiralMove = new SpiralMoveComponent
        {
            InitialVelocity = initialVelocity,
            AngularVelocity = angularVelocity,
            RadiusGrowth = radiusGrowth,
            CurrentAngle = 0,
            CenterPoint = position
        };

        var spriteRender = new RenderComponent();
        spriteRender.SetSprite(spriteFactory.CreateSprite(new CircleTexture(graphicDevice, circleProps)));

        AddComponent(transform);
        AddComponent(spiralMove);
        AddComponent(spriteRender);
    }
}
