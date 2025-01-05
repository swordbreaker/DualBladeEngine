using DualBlade._2D.BladePhysics.Components;
using DualBlade._2D.BladePhysics.Extensions;
using DualBlade._2D.BladePhysics.Models;
using DualBlade._2D.BladePhysics.Services;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;

namespace DualBlade._2D.BladePhysics.Systems;

public class DebugColliderSystem(IGameContext context) : ComponentSystem<ColliderComponent>(context)
{
    private readonly SpriteBatch spriteBatch = context.GameEngine.SpriteBatch;
    private readonly IGameEngine gameEngine = context.GameEngine;
    private readonly IWorldToPixelConverter worldToPixelConverter = context.GameEngine.WorldToPixelConverter;
    private readonly IPhysicsManager physicsManager = context.ServiceProvider.GetRequiredService<IPhysicsManager>();

    private readonly List<Action> drawActions = new();

    protected override void Draw(ColliderComponent component, IEntity entity, GameTime gameTime)
    {
        drawActions.Add(DrawGrid);

        if (entity.TryGetComponent<RigidBody>(out var body) && body.CollectCollisionEvents)
        {
            drawActions.Add(() => Draw(body));
        }

        foreach (var collider in component.Colliders)
        {
            drawActions.Add(() => Draw(collider));
        }
    }

    public override void Draw(GameTime gameTime)
    {
        drawActions.Clear();
    }

    public override void LateDraw(GameTime gameTime)
    {
        spriteBatch.Begin(SpriteSortMode.Deferred, transformMatrix: gameEngine.CameraService.PixelTransformMatrix);
        drawActions.ForEach(x => x());
        spriteBatch.End();
    }

    private void DrawGrid()
    {
        var p = (PhysicsManager)physicsManager;

        if (p.uniformGrid == null) return;

        for (var y = 0; y < p.uniformGrid.rows; y++)
        {
            for (var x = 0; x < p.uniformGrid.cols; x++)
            {
                var pos = new Vector2(x, y) * p.uniformGrid.cellSize - p.uniformGrid.offset;

                var scale = worldToPixelConverter.WorldSizeToPixel(new Vector2(p.uniformGrid.cellSize));
                var pixelPos = worldToPixelConverter.WorldPointToPixel(pos);

                var l = p.uniformGrid.grid[y, x].Count;

                var color = l switch
                {
                    0 => Color.Green,
                    1 => Color.Yellow,
                    2 => Color.Orange,
                    3 => Color.Red,
                    _ => Color.Purple
                };

                spriteBatch.DrawRectangle(new RectangleF(pixelPos, scale), color);
            }
        }
    }

    private void Draw(RigidBody rigidBody)
    {
        var collisions = physicsManager.GetNewCollisions(rigidBody);

        foreach (var collision in collisions)
        {
            var pos = worldToPixelConverter.WorldPointToPixel(collision.ContactPoint);
            spriteBatch.DrawCircle(pos, 5, 10, Color.Red);

            spriteBatch.DrawLine(pos, pos + collision.Normal * collision.PenetrationDepth, Color.Black, 3);
        }
    }

    private void Draw(ICollider collider)
    {
        switch (collider)
        {
            case CircleCollider circle:
                {
                    var pos = worldToPixelConverter.WorldPointToPixel(circle.Center + circle.Offset);
                    var radius = worldToPixelConverter.WorldSizeToPixel(new Vector2(circle.Radius * circle.Scale.X));

                    spriteBatch.DrawCircle(pos, radius.X, 10, Color.Yellow);
                }
                break;
            case RectangleCollider box:
                {
                    var bounds = box.AbsoluteBounds();
                    var pos = worldToPixelConverter.WorldPointToPixel(bounds.Location.ToVector2());
                    var scale = worldToPixelConverter.WorldSizeToPixel(bounds.Size.ToVector2());

                    pos -= new Vector2(0, scale.Y);
                    spriteBatch.DrawRectangle(new RectangleF(pos.X, pos.Y, scale.X, scale.Y),
                        Color.Purple);
                }
                break;
            case PolygonCollider polygon:
                {
                    var vertices = polygon.AbsoluteVertices.Select(x => worldToPixelConverter.WorldPointToPixel(x)).ToArray();

                    var poly = new Polygon(vertices);
                    spriteBatch.DrawPolygon(Vector2.Zero, poly, Color.Purple);
                }
                break;
        }
    }
}