using BulletHell.Desktop.Components;
using BulletHell.Desktop.Helpers;
using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;

namespace BulletHell.Desktop.Entities;

[RequiredComponent<TransformComponent>]
[RequiredComponent<MoveToComponent>]
[RequiredComponent<RenderComponent>]
public partial struct SpawnerEntity : IEntity
{
    public SpawnerEntity(Vector2 position, IGameContext context)
    {
        var transform = new TransformComponent
        {
            Position = position
        };

        var moveComponent = new MoveToComponent
        {
            TargetPosition = position,
            Speed = 1f // Set a default speed
        };

        var rectangleProperties = new RectangleProperties
        {
            Height = 50,
            Width = 50,
            StrokeColor = Color.Black,
            FillColor = Color.White,
            StrokeThickness = 1,
        };

        var RenderComponent = new RenderComponent
        {
            Sprite = context.GameEngine.SpriteFactory.CreateSprite(
                new RectangleTexture(
                    context.GameEngine.GraphicsDeviceManager.GraphicsDevice,
                    rectangleProperties) // Example color and size
            )
        };

        AddComponent(transform);
        AddComponent(moveComponent);
        AddComponent(RenderComponent);
    }
}
