using BulletHell.Desktop.Factories;
using DualBlade.Core.Services;
using DualBlade.Core.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace BulletHell.Desktop.Services;

public class BulletPatternService
{
    private readonly BulletFactory _bulletFactory;
    private readonly IGameContext _gameContext;
    private readonly Random _random = new();

    public BulletPatternService(IGameContext gameContext)
    {
        _gameContext = gameContext;
        _bulletFactory = new BulletFactory(gameContext);
    }

    /// <summary>
    /// Creates a danmaku-style pattern with many bullets
    /// </summary>
    public IEnumerable<IEntity> CreateDanmakuPattern(Vector2 centerPosition, float intensity)
    {
        var bullets = new List<IEntity>();
        var bulletCount = (int)(20 + intensity * 30); // 20-50 bullets
        var layers = (int)(2 + intensity * 3); // 2-5 layers

        for (int layer = 0; layer < layers; layer++)
        {
            var layerBulletCount = bulletCount / layers;
            var layerSpeed = 40f + layer * 20f;
            var layerOffset = layer * 0.2f; // Slight angle offset per layer

            for (int i = 0; i < layerBulletCount; i++)
            {
                var angle = (float)(i * MathHelper.TwoPi / layerBulletCount + layerOffset);
                var velocity = new Vector2(
                    (float)Math.Cos(angle) * layerSpeed,
                    (float)Math.Sin(angle) * layerSpeed
                );

                var color = Color.Lerp(Color.Red, Color.Yellow, layer / (float)layers);
                var bullet = _bulletFactory.CreateBullet(
                    BulletType.Linear,
                    centerPosition,
                    velocity,
                    color,
                    4 + layer
                );
                bullets.Add(bullet);
            }
        }

        return bullets;
    }

    /// <summary>
    /// Creates a flower pattern that blooms outward
    /// </summary>
    public IEnumerable<IEntity> CreateFlowerPattern(Vector2 centerPosition, int petalCount, float intensity)
    {
        var bullets = new List<IEntity>();
        var bulletsPerPetal = (int)(3 + intensity * 5);

        for (int petal = 0; petal < petalCount; petal++)
        {
            var petalAngle = petal * MathHelper.TwoPi / petalCount;

            for (int i = 0; i < bulletsPerPetal; i++)
            {
                var bulletAngle = petalAngle + (i - bulletsPerPetal / 2f) * 0.3f;
                var speed = 30f + i * 10f + intensity * 20f;

                var velocity = new Vector2(
                    (float)Math.Cos(bulletAngle) * speed,
                    (float)Math.Sin(bulletAngle) * speed
                );

                var color = Color.Lerp(Color.Pink, Color.Purple, i / (float)bulletsPerPetal);
                var bullet = _bulletFactory.CreateBullet(
                    BulletType.Wave,
                    centerPosition,
                    velocity,
                    color,
                    6
                );
                bullets.Add(bullet);
            }
        }

        return bullets;
    }

    /// <summary>
    /// Creates a spiral galaxy pattern
    /// </summary>
    public IEnumerable<IEntity> CreateSpiralGalaxyPattern(Vector2 centerPosition, float intensity)
    {
        var bullets = new List<IEntity>();
        var arms = 3; // Three-armed spiral
        var bulletsPerArm = (int)(8 + intensity * 12);

        for (int arm = 0; arm < arms; arm++)
        {
            var armOffset = arm * MathHelper.TwoPi / arms;

            for (int i = 0; i < bulletsPerArm; i++)
            {
                var t = i / (float)bulletsPerArm;
                var angle = armOffset + t * MathHelper.TwoPi * 2; // Two full rotations
                var radius = t * 100f;

                var position = centerPosition + new Vector2(
                    (float)Math.Cos(angle) * radius,
                    (float)Math.Sin(angle) * radius
                );

                var velocity = new Vector2(
                    (float)Math.Cos(angle + MathHelper.PiOver2) * (20f + intensity * 30f),
                    (float)Math.Sin(angle + MathHelper.PiOver2) * (20f + intensity * 30f)
                );

                var color = Color.Lerp(Color.Blue, Color.Cyan, t);
                var bullet = _bulletFactory.CreateBullet(
                    BulletType.Spiral,
                    position,
                    velocity,
                    color,
                    5
                );
                bullets.Add(bullet);
            }
        }

        return bullets;
    }

    /// <summary>
    /// Creates a curtain of bullets falling like rain
    /// </summary>
    public IEnumerable<IEntity> CreateRainCurtainPattern(Vector2 topPosition, float width, float intensity)
    {
        var bullets = new List<IEntity>();
        var bulletCount = (int)(10 + intensity * 20);
        var rows = (int)(2 + intensity * 3);

        for (int row = 0; row < rows; row++)
        {
            var rowY = topPosition.Y - row * 50f;
            var rowBulletCount = bulletCount / rows;

            for (int i = 0; i < rowBulletCount; i++)
            {
                var x = topPosition.X - width / 2 + (i * width / rowBulletCount) +
                       _random.NextSingle() * 20f - 10f; // Add some randomness

                var position = new Vector2(x, rowY);
                var velocity = new Vector2(
                    (_random.NextSingle() - 0.5f) * 20f, // Slight horizontal drift
                    -40f - intensity * 40f - row * 10f // Faster for later rows
                );

                var color = Color.Lerp(Color.LightBlue, Color.Blue, row / (float)rows);
                var bulletType = _random.NextSingle() > 0.8f ? BulletType.Wave : BulletType.Linear;

                var bullet = _bulletFactory.CreateBullet(
                    bulletType,
                    position,
                    velocity,
                    color,
                    4
                );
                bullets.Add(bullet);
            }
        }

        return bullets;
    }

    /// <summary>
    /// Creates a geometric pattern based on audio frequency analysis
    /// </summary>
    public IEnumerable<IEntity> CreateFrequencyPattern(Vector2 centerPosition, float[] frequencyBands)
    {
        var bullets = new List<IEntity>();
        var segments = Math.Min(frequencyBands.Length, 16); // Limit to 16 segments

        for (int i = 0; i < segments; i++)
        {
            var intensity = frequencyBands[i];
            if (intensity < 0.1f) continue; // Skip quiet frequencies

            var angle = i * MathHelper.TwoPi / segments;
            var distance = 50f + intensity * 100f;

            var position = centerPosition + new Vector2(
                (float)Math.Cos(angle) * distance,
                (float)Math.Sin(angle) * distance
            );

            var velocity = new Vector2(
                (float)Math.Cos(angle) * (30f + intensity * 50f),
                (float)Math.Sin(angle) * (30f + intensity * 50f)
            );

            // Color based on frequency (low = red, high = blue)
            var hue = i / (float)segments;
            var color = Color.Lerp(Color.Red, Color.Blue, hue);

            var bulletType = intensity > 0.7f ? BulletType.Homing :
                           intensity > 0.4f ? BulletType.Accelerating : BulletType.Linear;

            var bullet = _bulletFactory.CreateBullet(
                bulletType,
                position,
                velocity,
                color,
                (int)(5 + intensity * 10)
            );
            bullets.Add(bullet);
        }

        return bullets;
    }

    /// <summary>
    /// Creates a chaotic pattern for intense moments
    /// </summary>
    public IEnumerable<IEntity> CreateChaosPattern(Vector2 centerPosition, float intensity)
    {
        var bullets = new List<IEntity>();
        var bulletCount = (int)(15 + intensity * 35);

        for (int i = 0; i < bulletCount; i++)
        {
            var angle = _random.NextSingle() * MathHelper.TwoPi;
            var speed = _random.NextSingle() * (50f + intensity * 100f) + 20f;

            var velocity = new Vector2(
                (float)Math.Cos(angle) * speed,
                (float)Math.Sin(angle) * speed
            );

            var position = centerPosition + new Vector2(
                (_random.NextSingle() - 0.5f) * 100f,
                (_random.NextSingle() - 0.5f) * 100f
            );

            var bulletTypes = new[] {
                BulletType.Linear, BulletType.Bounce, BulletType.Wave,
                BulletType.Spiral, BulletType.Accelerating, BulletType.Zigzag,
                BulletType.Pulsing,
            };
            var bulletType = bulletTypes[_random.Next(bulletTypes.Length)];

            var color = new Color(
                _random.NextSingle(),
                _random.NextSingle(),
                _random.NextSingle()
            );

            var bullet = _bulletFactory.CreateBullet(
                bulletType,
                position,
                velocity,
                color,
                _random.Next(4, 12)
            );
            bullets.Add(bullet);
        }

        return bullets;
    }

    /// <summary>
    /// Creates a magnetic storm pattern with gravitational and magnetic bullets
    /// </summary>
    public IEnumerable<IEntity> CreateMagneticStormPattern(Vector2 centerPosition, float intensity)
    {
        var bullets = new List<IEntity>();
        var magneticCenters = (int)(2 + intensity * 3); // 2-5 magnetic centers

        for (int center = 0; center < magneticCenters; center++)
        {
            var centerAngle = center * MathHelper.TwoPi / magneticCenters;
            var centerDistance = 80f + intensity * 40f;

            var magneticCenter = centerPosition + new Vector2(
                (float)Math.Cos(centerAngle) * centerDistance,
                (float)Math.Sin(centerAngle) * centerDistance
            );

            // Create gravitational bullets around each center
            var bulletsPerCenter = (int)(4 + intensity * 6);
            for (int i = 0; i < bulletsPerCenter; i++)
            {
                var angle = i * MathHelper.TwoPi / bulletsPerCenter;
                var distance = 30f + _random.NextSingle() * 20f;

                var position = magneticCenter + new Vector2(
                    (float)Math.Cos(angle) * distance,
                    (float)Math.Sin(angle) * distance
                );

                var velocity = new Vector2(
                    (float)Math.Cos(angle + MathHelper.PiOver2) * (30f + intensity * 20f),
                    (float)Math.Sin(angle + MathHelper.PiOver2) * (30f + intensity * 20f)
                );

                var color = Color.Lerp(Color.Purple, Color.Magenta, intensity);
                var bullet = _bulletFactory.CreateBullet(
                    BulletType.Gravitational,
                    position,
                    velocity,
                    color,
                    6
                );
                bullets.Add(bullet);
            }
        }

        return bullets;
    }

    /// <summary>
    /// Creates a chain reaction pattern with chain and cluster bullets
    /// </summary>
    public IEnumerable<IEntity> CreateChainReactionPattern(Vector2 centerPosition, float intensity)
    {
        var bullets = new List<IEntity>();
        var chainCount = (int)(2 + intensity * 3);

        for (int chain = 0; chain < chainCount; chain++)
        {
            var angle = chain * MathHelper.TwoPi / chainCount;
            var distance = 60f + intensity * 40f;

            var position = centerPosition + new Vector2(
                (float)Math.Cos(angle) * distance,
                (float)Math.Sin(angle) * distance
            );

            var velocity = new Vector2(
                (float)Math.Cos(angle) * (40f + intensity * 30f),
                (float)Math.Sin(angle) * (40f + intensity * 30f)
            );

            // Alternate between chain and cluster bullets
            var bulletType = chain % 2 == 0 ? BulletType.Chain : BulletType.Cluster;
            var color = bulletType == BulletType.Chain ? Color.Orange : Color.Red;

            var bullet = _bulletFactory.CreateBullet(
                bulletType,
                position,
                velocity,
                color,
                8
            );
            bullets.Add(bullet);
        }

        return bullets;
    }

    /// <summary>
    /// Creates a pulse wave pattern with pulsing bullets
    /// </summary>
    public IEnumerable<IEntity> CreatePulseWavePattern(Vector2 centerPosition, float intensity)
    {
        var bullets = new List<IEntity>();
        var rings = (int)(3 + intensity * 2);
        var bulletsPerRing = 12;

        for (int ring = 0; ring < rings; ring++)
        {
            var ringRadius = (ring + 1) * 50f;
            var ringDelay = ring * 0.5f; // Stagger the pulses

            for (int i = 0; i < bulletsPerRing; i++)
            {
                var angle = i * MathHelper.TwoPi / bulletsPerRing;
                var position = centerPosition + new Vector2(
                    (float)Math.Cos(angle) * ringRadius,
                    (float)Math.Sin(angle) * ringRadius
                );

                var velocity = new Vector2(
                    (float)Math.Cos(angle) * (25f + intensity * 25f),
                    (float)Math.Sin(angle) * (25f + intensity * 25f)
                );

                var color = Color.Lerp(Color.Yellow, Color.Orange, ring / (float)rings);
                var bullet = _bulletFactory.CreateBullet(
                    BulletType.Pulsing,
                    position,
                    velocity,
                    color,
                    7
                );
                bullets.Add(bullet);
            }
        }

        return bullets;
    }

    /// <summary>
    /// Creates a seeking swarm pattern
    /// </summary>
    public IEnumerable<IEntity> CreateSeekingSwarmPattern(Vector2 centerPosition, float intensity)
    {
        var bullets = new List<IEntity>();
        var swarmSize = (int)(6 + intensity * 10);

        for (int i = 0; i < swarmSize; i++)
        {
            var angle = _random.NextSingle() * MathHelper.TwoPi;
            var distance = _random.NextSingle() * 80f + 40f;

            var position = centerPosition + new Vector2(
                (float)Math.Cos(angle) * distance,
                (float)Math.Sin(angle) * distance
            );

            var velocity = new Vector2(
                (_random.NextSingle() - 0.5f) * 40f,
                (_random.NextSingle() - 0.5f) * 40f
            );

            var color = Color.Lerp(Color.LightGreen, Color.DarkGreen, _random.NextSingle());
            var bullet = _bulletFactory.CreateBullet(
                BulletType.Seeking,
                position,
                velocity,
                color,
                5
            );
            bullets.Add(bullet);
        }

        return bullets;
    }
}
