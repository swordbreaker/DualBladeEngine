using DualBlade._2D.Physics.Components;
using DualBlade._2D.Physics.Services;
using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using FluidBattle.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;

namespace FluidBattle.Entities;

[AddComponent<FluidComponent>]
public partial struct FluidPixelEntityOld : IEntity
{
    public FluidPixelEntityOld(IGameContext context, Vector2 pos)
    {
        var physicsManager = context.ServiceProvider.GetRequiredService<IPhysicsManager>();
        var spriteFactory = context.GameEngine.SpriteFactory;

        var renderComponent = new RenderComponent();
        renderComponent.SetSprite(spriteFactory.CreateWhitePixelSprite());
        renderComponent.Color = Color.Red;

        var scale = 4;
        var transform = new TransformComponent() { Position = pos, Scale = Vector2.One * scale };

        // Create a physics body
        var size = renderComponent.Sprite.Size * scale;

        var body = physicsManager.CreateBody(transform.Position, bodyType: BodyType.Dynamic);
        body.IgnoreGravity = true;
        body.Tag = this;
        body.FixedRotation = true;
        body.Mass = 1;
        body.LinearDamping = 0f;
        var fixture = body.CreateRectangle(size.X, size.Y, 1, Vector2.Zero);
        fixture.Tag = this;
        fixture.Restitution = 0.01f;
        fixture.Friction = 0f;

        var kinematic = new KinematicComponent
        {
            PhysicsBody = body
        };

        AddComponent(transform);
        AddComponent(renderComponent);
        AddComponent(kinematic);
    }
}
