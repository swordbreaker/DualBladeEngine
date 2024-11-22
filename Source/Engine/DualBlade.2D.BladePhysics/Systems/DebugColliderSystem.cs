using DualBlade._2D.BladePhysics.Components;
using DualBlade._2D.BladePhysics.Extensions;
using DualBlade._2D.BladePhysics.Models;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Particles.Profiles;

namespace DualBlade._2D.BladePhysics.Systems;

public class DebugColliderSystem(IGameContext context) : ComponentSystem<ColliderComponent>(context)
{
    private readonly SpriteBatch spriteBatch = context.GameEngine.SpriteBatch;
    private readonly IGameEngine gameEngine = context.GameEngine;
    private readonly IWorldToPixelConverter worldToPixelConverter = context.GameEngine.WorldToPixelConverter;

    protected override void Draw(ColliderComponent component, IEntity entity, GameTime gameTime)
    {
        foreach (var collider in component.Colliders)
        {
            Draw(collider);
        }
    }

    public override void Draw(GameTime gameTime)
    {
        spriteBatch.Begin(SpriteSortMode.Deferred, transformMatrix: gameEngine.CameraService.PixelTransformMatrix);
    }

    public override void LateDraw(GameTime gameTime)
    {
        spriteBatch.End();
    }

    private void Draw(ICollider collider)
    {
        switch (collider)
        {
            case CircleCollider circle:
            {
                var pos = worldToPixelConverter.WorldPointToPixel(circle.Center + circle.Offset);

                spriteBatch.DrawCircle(pos, circle.Radius * circle.Scale.X, 10, Color.Purple);
            }
                break;
            case RectangleCollider box:
            {
                var bounds = box.AbsoluteBounds();
                var pos = worldToPixelConverter.WorldPointToPixel(bounds.Location.ToVector2());
                var scale = worldToPixelConverter.WorldSizeToPixel(bounds.Size.ToVector2());
                spriteBatch.DrawRectangle(new RectangleF(pos.X, pos.Y, scale.X, scale.Y),
                    Color.Purple);
            }
                break;
        }
    }
}