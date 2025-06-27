using BulletHell.Desktop.Components;
using BulletHell.Desktop.Helpers;
using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;

namespace BulletHell.Desktop.Entities;

public partial struct PlayerEntity : IEntity
{
    public PlayerEntity(Vector2 position, IGameContext gameContext)
    {

        var texture = new CircleTexture(
            gameContext.GameEngine.GraphicsDeviceManager.GraphicsDevice,
            new CircleProperties
            {
                Radius = 10,
                FillColor = Color.Green,
                StrokeColor = Color.White,
            });

        var sprite = gameContext.GameEngine.SpriteFactory.CreateSprite(texture);

        var renderer = new RenderComponent();
        renderer.SetSprite(sprite);

        AddComponent(new TransformComponent { Position = position });
        AddComponent(renderer);
        AddComponent(new PlayerComponent());
    }

    public IGameContext GameContext { get; }
    public Entity Entity { get; }
}
