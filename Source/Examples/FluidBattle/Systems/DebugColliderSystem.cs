using DualBlade._2D.Physics.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using nkast.Aether.Physics2D.Collision.Shapes;
using System.Linq;

namespace FluidBattle.Systems;

public class DebugColliderSystem(IGameContext context) : ComponentSystem<KinematicComponent>(context)
{
    private readonly SpriteBatch spriteBatch = context.GameEngine.SpriteBatch;
    private readonly IWorldToPixelConverter worldToPixelConverter = context.GameEngine.WorldToPixelConverter;

    public override void Draw(GameTime gameTime)
    {
    }

    public override void LateDraw(GameTime gameTime)
    {
    }

    protected override void Draw(KinematicComponent component, IEntity entity, GameTime gameTime)
    {
        var body = component.PhysicsBody;
        var fixture = body.FixtureList.First();
        spriteBatch.Begin();

        switch (fixture.Shape)
        {
            case CircleShape circle:
                var pos = worldToPixelConverter.WorldPointToPixel(body.Position);
                var radius = circle.Radius * worldToPixelConverter.TileSize;
                spriteBatch.DrawCircle(new CircleF(pos, radius), 12, Color.Green, thickness: 1f);
                break;
        }

        spriteBatch.End();
    }
}
