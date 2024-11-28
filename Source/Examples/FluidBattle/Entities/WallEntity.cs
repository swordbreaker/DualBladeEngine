using DualBlade._2D.BladePhysics.Components;
using DualBlade._2D.BladePhysics.Models;
using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;

namespace FluidBattle.Entities;

public partial struct WallEntity : IEntity
{
    public WallEntity(Vector2 pos, Vector2 size, IGameContext context)
    {
        var transform = new TransformComponent
        {
            Position = pos,
            Scale = size
        };

        var render = new RenderComponent
        {
            Color = Color.Black
        };

        var sprite = context.GameEngine.SpriteFactory.CreateWhitePixelSprite();
        render.SetSprite(sprite);

        var collider = new RectangleCollider(Zero, context.GameEngine.WorldToPixelConverter.PixelSizeToWorld(Vector2.One));

        AddComponent(transform);
        AddComponent(render);
        AddComponent(new ColliderComponent(collider));
    }
}