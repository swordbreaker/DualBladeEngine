using DualBlade._2D.Physics.Components;
using DualBlade._2D.Physics.Services;
using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;

namespace FluidBattle.Entities;
public partial struct WallEntity : IEntity
{
    public WallEntity(Vector2 pos, Vector2 size, IGameContext context)
    {
        var spriteFactory = context.GameEngine.SpriteFactory;
        var physicsManager = context.ServiceProvider.GetRequiredService<IPhysicsManager>();
        var worldToPixelConverter = context.GameEngine.WorldToPixelConverter;

        var transform = new TransformComponent
        {
            Position = pos,
            Scale = size
        };

        var render = new RenderComponent
        {
            Color = Color.Black
        };

        render.SetSprite(context.GameEngine.SpriteFactory.CreateWhitePixelSprite());

        var pixelSize = render.Sprite.Size * size;

        var body = physicsManager.CreateBody(transform.Position, bodyType: BodyType.Static);
        body.IgnoreGravity = true;

        var fixture = body.CreateRectangle(pixelSize.X, pixelSize.Y, 1, Vector2.Zero);

        var kinematic = new KinematicComponent
        {
            PhysicsBody = body
        };

        AddComponent(transform);
        AddComponent(render);
        AddComponent(kinematic);
    }
}
