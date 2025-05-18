using System;
using BulletHell.Desktop.Components;
using BulletHell.Desktop.Entities;
using BulletHell.Desktop.Helpers;
using BulletHell.Desktop.Models;
using BulletHell.Desktop.Services;
using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using Microsoft.Extensions.DependencyInjection;

namespace BulletHell.Desktop.Systems;

public class SpawnerSystem(IGameContext context) : EntitySystem<SpawnerEntity>(context)
{
    private MusicContext musicContext;

    private Vector2 GameSize => GameContext.GameEngine.GameSize;

    public override void Initialize()
    {
        base.Initialize();

        musicContext = GameContext.ServiceProvider.GetRequiredService<MusicContext>();
    }

    protected override void Update(ref SpawnerEntity entity, GameTime gameTime)
    {
        if (!musicContext.FeatureConverter.IsReady) return;

        var pos = entity.TransformComponentCopy.Position;

        if (Vector2.DistanceSquared(
            entity.MoveToComponentCopy.TargetPosition,
            pos) < 0.1f)
        {
            var centroid = musicContext.FeatureConverter.CurrentNormalizedCentroid;
            var x = (centroid * GameSize.X) - (GameSize.X / 2);
            var y = (GameSize.Y / 2) - (centroid * GameSize.Y / 2);

            entity.UpdateComponent<MoveToComponent>((c) =>
            {
                c.TargetPosition = new Vector2(x, y);
                c.Speed = 2f;
                return c;
            });
        }

        entity.UpdateComponent<RenderComponent>((c) =>
        {
            if (musicContext.FeatureConverter.IsBeat(1))
            {
                var decrease = musicContext.FeatureConverter.CurrentNormalizedDecrease;

                SpawnBullentInACirlce(pos, 20, (float)gameTime.TotalGameTime.TotalSeconds, decrease);
                c.Color = Color.White;
            }
            else
            {
                c.Color = Color.Blue;
            }

            return c;
        });
    }

    private void SpawnBullentInACirlce(
        Vector2 position,
        int count,
        float offset,
        float horizontalVelocity = 0)
    {
        var angleStep = MathHelper.TwoPi / count;

        for (int i = 0; i < count; i++)
        {
            var angle = i * angleStep;
            var velocity = new Vector2(MathF.Cos(angle + offset), MathF.Sin(angle + offset)) * 10f;
            var normal = Vector2.Normalize(new Vector2(-velocity.X, velocity.Y));
            velocity += normal * (1 - horizontalVelocity);

            SpanwBullet(position, velocity);
        }
    }

    private void SpanwBullet(Vector2 position, Vector2 velocity)
    {
        var cirlceProps = new CircleProperties
        {
            Radius = 10,
            StrokeColor = Color.Black,
            FillColor = Color.Red,
        };

        var specPoint = new SpectogramPoint
        {
            Point = position,
            Frequency = 0,
            Decibel = 0,
            RelativeValue = 0
        };

        var bullet = new BulletEntity(specPoint, velocity, cirlceProps, GameContext);

        World.AddEntity(bullet);
    }
}
