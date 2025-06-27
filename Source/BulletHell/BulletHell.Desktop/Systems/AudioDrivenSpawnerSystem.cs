using System.Linq;
using BulletHell.Desktop.Entities;
using BulletHell.Desktop.Factories;
using BulletHell.Desktop.Services;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using Microsoft.Extensions.DependencyInjection;

namespace BulletHell.Desktop.Systems;

public class AudioDrivenSpawnerSystem(IGameContext context) : BaseSystem(context)
{
    private readonly BulletFactory _bulletFactory = new(context);
    private MusicContext _musicContext;
    private PlayerEntity? _player;
    private readonly Random _random = new();

    // Timing controls
    private float _lastSpawnTime = 0f;
    private float _beatSpawnCooldown = 0.2f; // Minimum time between beat-triggered spawns
    private float _regularSpawnInterval = 0.5f; // Regular spawn interval when no beats
    private float _lastRegularSpawn = 0f;

    public override void Initialize()
    {
        base.Initialize();
        _musicContext = GameContext.ServiceProvider.GetRequiredService<MusicContext>();

        // Try to find player entity
        FindPlayer();
    }

    public override void Update(GameTime gameTime)
    {
        if (!_musicContext.FeatureConverter.IsReady)
            return;

        var currentTime = (float)gameTime.TotalGameTime.TotalSeconds;
        var features = GetCurrentMusicFeatures();

        _player = Ecs.SingelEntity<PlayerEntity>();

        // Spawn bullets based on beats
        if (ShouldSpawnOnBeat(features, currentTime))
        {
            // SpawnBeatPatterns(features);
            SpawnRegularPattern(features);
            _lastSpawnTime = currentTime;
        }
        // Regular spawning for continuous action
        else if (currentTime - _lastRegularSpawn > _regularSpawnInterval)
        {
            SpawnRegularPattern(features);
            _lastRegularSpawn = currentTime;
        }

        // Spawn special patterns based on audio features
        // SpawnSpecialPatterns(features, currentTime);
    }

    private bool ShouldSpawnOnBeat(MusicFeatures features, float currentTime)
    {
        return features.IsBeat && (currentTime - _lastSpawnTime) > _beatSpawnCooldown;
    }

    private void SpawnBeatPatterns(MusicFeatures features)
    {
        var gameSize = GameContext.GameEngine.GameSize;
        var intensity = features.RMS * 3;

        if (intensity > 0.6f)
        {
            // Medium intensity: Circle burst
            SpawnCircleBurst(features);
        }
        else if (intensity > 0.4f)
        {
            // Low intensity: Wave pattern
            SpawnWavePattern(features);
        }
        else
        {
            // Very low: Simple bullets
            SpawnSimplePattern(features);
        }
    }

    private void SpawnRegularPattern(MusicFeatures features)
    {
        // Spawn individual bullets based on frequency spectrum
        var gameSize = GameContext.GameEngine.GameSize;
        var position = new Vector2(
            0,
            gameSize.Y / 2 // Spawn from top
        );

        var bullet = _bulletFactory.CreateBulletFromAudioFeatures(position, features, _player);
        World.AddEntity(bullet);
    }

    private void SpawnSpecialPatterns(MusicFeatures features, float currentTime)
    {
        // Homing bullets on high centroid
        if (features.Centroid > 0.8f && _random.NextSingle() < 0.1f)
        {
            SpawnHomingBullet(features);
        }

        // Cluster bullets on high spread
        if (features.Spread > 0.7f && _random.NextSingle() < 0.05f)
        {
            SpawnClusterBullet(features);
        }
    }

    private void SpawnCircleBurst(MusicFeatures features)
    {
        var gameSize = GameContext.GameEngine.GameSize;
        var position = new Vector2(0, gameSize.Y / 4);
        var bulletCount = 8 + (int)(features.RMS * 12); // 8-20 bullets based on intensity

        var angleStep = MathHelper.TwoPi / bulletCount;
        var speed = 5f + features.RMS * 10f;

        for (int i = 0; i < bulletCount; i++)
        {
            var angle = i * angleStep;
            var velocity = new Vector2(
                MathF.Cos(angle) * speed,
                MathF.Sin(angle) * speed
            );

            var bulletType = features.Centroid > 0.5f ? BulletType.Accelerating : BulletType.Linear;
            var bullet = _bulletFactory.CreateBullet(
                bulletType,
                position,
                velocity,
                GetColorFromFeatures(features),
                6,
                _player
            );
            World.AddEntity(bullet);
        }
    }

    private void SpawnWavePattern(MusicFeatures features)
    {
        var gameSize = GameContext.GameEngine.GameSize;
        var bulletCount = 5;
        var spacing = gameSize.X / (bulletCount + 1);

        for (int i = 0; i < bulletCount; i++)
        {
            var position = new Vector2(
                -gameSize.X / 2 + spacing * (i + 1),
                gameSize.Y / 2
            );

            var velocity = new Vector2(0, -20f - features.RMS * 10f);
            var bullet = _bulletFactory.CreateBullet(
                BulletType.Wave,
                position,
                velocity,
                GetColorFromFeatures(features),
                7,
                _player
            );
            World.AddEntity(bullet);
        }
    }

    private void SpawnSimplePattern(MusicFeatures features)
    {
        var gameSize = GameContext.GameEngine.GameSize;
        var bulletCount = 2 + (int)(features.RMS * 3);

        for (int i = 0; i < bulletCount; i++)
        {
            var position = new Vector2(
                0,
                gameSize.Y / 2
            );

            var velocity = new Vector2(
                (features.Spread * i - 0.5f) * 5f,
                -5f - features.RMS * 10f
            );

            var bullet = _bulletFactory.CreateBullet(
                BulletType.Linear,
                position,
                velocity,
                GetColorFromFeatures(features),
                5,
                _player
            );
            World.AddEntity(bullet);
        }
    }

    private void SpawnHomingBullet(MusicFeatures features)
    {
        var gameSize = GameContext.GameEngine.GameSize;
        var position = new Vector2(
            (_random.NextSingle() - 0.5f) * gameSize.X,
            gameSize.Y / 2
        );

        var velocity = new Vector2(0, -15f);
        var bullet = _bulletFactory.CreateBullet(
            BulletType.Homing,
            position,
            velocity,
            Color.Red,
            12,
            _player
        );
        World.AddEntity(bullet);
    }

    private void SpawnClusterBullet(MusicFeatures features)
    {
        var gameSize = GameContext.GameEngine.GameSize;
        var position = new Vector2(0, gameSize.Y / 3);
        var velocity = new Vector2(0, -25f);

        var bullet = _bulletFactory.CreateBullet(
            BulletType.Cluster,
            position,
            velocity,
            Color.Yellow,
            15,
            _player
        );
        World.AddEntity(bullet);
    }

    private MusicFeatures GetCurrentMusicFeatures()
    {
        var converter = _musicContext.FeatureConverter;
        return new MusicFeatures
        {
            Centroid = converter.CurrentNormalizedCentroid,
            RMS = converter.CurrentRMS,
            Spread = converter.CurrentNormalizedDecrease, // Using decrease as spread proxy
            IsBeat = converter.IsBeat(0)
        };
    }

    private Color GetColorFromFeatures(MusicFeatures features)
    {
        // Create dynamic colors based on audio features
        var r = (byte)Math.Clamp(features.Centroid * 255, 50, 255);
        var g = (byte)Math.Clamp(features.RMS * 255, 50, 255);
        var b = (byte)Math.Clamp(features.Spread * 255, 50, 255);

        return new Color(r, g, b);
    }

    private void FindPlayer()
    {
        // This would need to be implemented based on how you track the player
        // For now, we'll leave it null and the bullets won't home in
        _player = null;

        World.Entities.FirstOrDefault(e =>
        {
            if (e is PlayerEntity player)
            {
                _player = player;
                return true;
            }
            return false;
        });
    }
}
