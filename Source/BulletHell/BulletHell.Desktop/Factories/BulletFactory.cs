using BulletHell.Desktop.Components;
using BulletHell.Desktop.Entities;
using BulletHell.Desktop.Helpers;
using BulletHell.Desktop.Models;
using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BulletHell.Desktop.Factories;

public class BulletFactory
{
    private readonly IGameContext _gameContext;
    private readonly Random _random;

    public BulletFactory(IGameContext gameContext)
    {
        _gameContext = gameContext;
        _random = new Random();
    }

    /// <summary>
    /// Creates a bullet based on audio features for dynamic generation
    /// </summary>
    public IEntity CreateBulletFromAudioFeatures(
        Vector2 position,
        MusicFeatures features,
        PlayerEntity? player = null)
    {
        // Map audio features to bullet types
        var bulletType = DetermineBulletTypeFromAudio(features);
        var velocity = CalculateVelocityFromAudio(features);
        var color = GetColorFromAudio(features);
        var size = GetSizeFromAudio(features);

        return CreateBullet(bulletType, position, velocity, color, size, player);
    }

    /// <summary>
    /// Create a specific bullet type
    /// </summary>
    public IEntity CreateBullet(
        BulletType bulletType,
        Vector2 position,
        Vector2 velocity,
        Color color,
        int size = 10,
        PlayerEntity? targetPlayer = null)
    {
        Console.WriteLine($"Creating bullet: {bulletType} at {position} with velocity {velocity}");

        var circleProps = new CircleProperties
        {
            Radius = size,
            StrokeColor = Color.Black,
            FillColor = color,
        };

        var specPoint = new SpectogramPoint
        {
            Point = position,
            Frequency = 0,
            Decibel = 0,
            RelativeValue = 0
        };

        return bulletType switch
        {
            BulletType.Linear => CreateLinearBullet(specPoint, velocity, circleProps),
            BulletType.Homing => CreateHomingBullet(specPoint, velocity, circleProps, targetPlayer),
            BulletType.Spiral => CreateSpiralBullet(specPoint, velocity, circleProps),
            BulletType.Wave => CreateWaveBullet(specPoint, velocity, circleProps),
            BulletType.Bounce => CreateBounceBullet(specPoint, velocity, circleProps),
            BulletType.Rocket => CreateRocketBullet(specPoint, velocity, circleProps, targetPlayer),
            BulletType.Laser => CreateLaserBullet(specPoint, velocity, circleProps),
            BulletType.Cluster => CreateClusterBullet(specPoint, velocity, circleProps),
            BulletType.Accelerating => CreateAcceleratingBullet(specPoint, velocity, circleProps),
            BulletType.Orbiting => CreateOrbitingBullet(specPoint, velocity, circleProps),
            BulletType.Zigzag => CreateZigzagBullet(specPoint, velocity, circleProps),
            BulletType.Pulsing => CreatePulsingBullet(specPoint, velocity, circleProps),
            BulletType.Gravitational => CreateGravitationalBullet(specPoint, velocity, circleProps, targetPlayer),
            BulletType.Seeking => CreateSeekingBullet(specPoint, velocity, circleProps, targetPlayer),
            BulletType.Chain => CreateChainBullet(specPoint, velocity, circleProps),
            _ => CreateLinearBullet(specPoint, velocity, circleProps)
        };
    }

    private IEntity CreateLinearBullet(SpectogramPoint point, Vector2 velocity, CircleProperties props)
    {
        return new BulletEntity(point, velocity, props, _gameContext);
    }

    private IEntity CreateHomingBullet(SpectogramPoint point, Vector2 velocity, CircleProperties props, PlayerEntity? target)
    {
        var entity = new Entity();

        var transform = new TransformComponent { Position = point.Point };
        var render = new RenderComponent();
        render.SetSprite(_gameContext.GameEngine.SpriteFactory.CreateSprite(
            new CircleTexture(_gameContext.GameEngine.GraphicsDeviceManager.GraphicsDevice, props)));

        Vector2 targetPosition = Vector2.Zero;
        bool hasTarget = false;
        if (target.HasValue && target.Value.TryGetComponent<TransformComponent>(out var targetTransform))
        {
            targetPosition = targetTransform.Position;
            hasTarget = true;
        }

        var homing = new HomingMoveComponent
        {
            Velocity = velocity,
            TargetPosition = targetPosition,
            TurnSpeed = 2.0f,
            MaxSpeed = velocity.Length(),
            AcquisitionRange = 200f,
            HasTarget = hasTarget
        };

        entity.AddComponent(transform);
        entity.AddComponent(render);
        entity.AddComponent(homing);
        entity.AddComponent(new DestroyOnScreenBoundsComponent());

        return entity;
    }

    private IEntity CreateSpiralBullet(SpectogramPoint point, Vector2 velocity, CircleProperties props)
    {
        var entity = new Entity();

        var transform = new TransformComponent { Position = point.Point };
        var render = new RenderComponent();
        render.SetSprite(_gameContext.GameEngine.SpriteFactory.CreateSprite(
            new CircleTexture(_gameContext.GameEngine.GraphicsDeviceManager.GraphicsDevice, props)));

        var spiral = new SpiralMoveComponent
        {
            InitialVelocity = velocity,
            AngularVelocity = MathHelper.ToRadians(90), // 90 degrees per second
            RadiusGrowth = 20f, // Radius grows by 20 units per second
            CurrentAngle = 0,
            CenterPoint = point.Point
        };

        entity.AddComponent(transform);
        entity.AddComponent(render);
        entity.AddComponent(spiral);
        entity.AddComponent(new DestroyOnScreenBoundsComponent());

        return entity;
    }

    private IEntity CreateWaveBullet(SpectogramPoint point, Vector2 velocity, CircleProperties props)
    {
        var entity = new Entity();

        var transform = new TransformComponent { Position = point.Point };
        var render = new RenderComponent();
        render.SetSprite(_gameContext.GameEngine.SpriteFactory.CreateSprite(
            new CircleTexture(_gameContext.GameEngine.GraphicsDeviceManager.GraphicsDevice, props)));

        var wave = new WaveMoveComponent
        {
            Direction = Vector2.Normalize(velocity),
            Speed = velocity.Length(),
            Amplitude = 2f,
            Frequency = 10f,
            Phase = _random.NextSingle() * MathHelper.TwoPi,
            PerpendicularDirection = Vector2.Normalize(new Vector2(-velocity.Y, velocity.X))
        };

        entity.AddComponent(transform);
        entity.AddComponent(render);
        entity.AddComponent(wave);
        entity.AddComponent(new DestroyOnScreenBoundsComponent());

        return entity;
    }

    private IEntity CreateBounceBullet(SpectogramPoint point, Vector2 velocity, CircleProperties props)
    {
        var entity = new Entity();

        var transform = new TransformComponent { Position = point.Point };
        var render = new RenderComponent();
        render.SetSprite(_gameContext.GameEngine.SpriteFactory.CreateSprite(
            new CircleTexture(_gameContext.GameEngine.GraphicsDeviceManager.GraphicsDevice, props)));

        var bounce = new BounceMoveComponent
        {
            Velocity = velocity,
            BouncesRemaining = 3,
            BounceDamping = 0.8f,
            FlipOnBounce = true
        };

        entity.AddComponent(transform);
        entity.AddComponent(render);
        entity.AddComponent(bounce);
        // Note: Don't add DestroyOnScreenBoundsComponent for bouncing bullets

        return entity;
    }

    private IEntity CreateRocketBullet(SpectogramPoint point, Vector2 velocity, CircleProperties props, PlayerEntity? target)
    {
        if (!target.HasValue || !target.Value.TryGetComponent<TransformComponent>(out var targetTransform))
        {
            // Handle case where target is not valid
            targetTransform = new TransformComponent { Position = Vector2.Zero };
        }

        return new RocketBulletEntity(point.Point, targetTransform, velocity * 0.5f, _gameContext);
    }

    private IEntity CreateLaserBullet(SpectogramPoint point, Vector2 velocity, CircleProperties props)
    {
        // Create a fast, thin bullet
        var laserProps = new CircleProperties
        {
            Radius = props.Radius / 2,
            StrokeColor = Color.White,
            FillColor = Color.Cyan,
        };

        var fastVelocity = velocity * 3f; // Make lasers faster
        return new BulletEntity(point, fastVelocity, laserProps, _gameContext);
    }

    private IEntity CreateClusterBullet(SpectogramPoint point, Vector2 velocity, CircleProperties props)
    {
        // This creates the main bullet that will split into smaller ones
        var entity = new Entity();

        var transform = new TransformComponent { Position = point.Point };
        var render = new RenderComponent();
        render.SetSprite(_gameContext.GameEngine.SpriteFactory.CreateSprite(
            new CircleTexture(_gameContext.GameEngine.GraphicsDeviceManager.GraphicsDevice, props)));

        var move = new MoveComponent { Velocity = velocity };

        // Add a cluster component that will be handled by a system
        var cluster = new ClusterBulletComponent
        {
            SplitDistance = 100f,
            ClusterCount = 5,
            HasSplit = false
        };

        entity.AddComponent(transform);
        entity.AddComponent(render);
        entity.AddComponent(move);
        entity.AddComponent(cluster);
        entity.AddComponent(new DestroyOnScreenBoundsComponent());

        return entity;
    }

    private IEntity CreateAcceleratingBullet(SpectogramPoint point, Vector2 velocity, CircleProperties props)
    {
        var entity = new Entity();

        var transform = new TransformComponent { Position = point.Point };
        var render = new RenderComponent();
        render.SetSprite(_gameContext.GameEngine.SpriteFactory.CreateSprite(
            new CircleTexture(_gameContext.GameEngine.GraphicsDeviceManager.GraphicsDevice, props)));

        var accel = new AccelerationMoveComponent
        {
            Velocity = velocity * 0.3f, // Start slow
            Acceleration = Vector2.Normalize(velocity) * 200f, // Accelerate in direction
            MaxSpeed = velocity.Length() * 2f
        };

        entity.AddComponent(transform);
        entity.AddComponent(render);
        entity.AddComponent(accel);
        entity.AddComponent(new DestroyOnScreenBoundsComponent());

        return entity;
    }

    private IEntity CreateOrbitingBullet(SpectogramPoint point, Vector2 velocity, CircleProperties props)
    {
        var entity = new Entity();

        var transform = new TransformComponent { Position = point.Point };
        var render = new RenderComponent();
        render.SetSprite(_gameContext.GameEngine.SpriteFactory.CreateSprite(
            new CircleTexture(_gameContext.GameEngine.GraphicsDeviceManager.GraphicsDevice, props)));

        var orbit = new OrbitMoveComponent
        {
            CenterPoint = point.Point,
            OrbitRadius = 50f,
            OrbitSpeed = 2f,
            CurrentAngle = _random.NextSingle() * MathHelper.TwoPi,
            MoveSpeed = velocity.Length() * 0.5f, // Move toward player while orbiting
            Direction = Vector2.Normalize(velocity)
        };

        entity.AddComponent(transform);
        entity.AddComponent(render);
        entity.AddComponent(orbit);
        entity.AddComponent(new DestroyOnScreenBoundsComponent());

        return entity;
    }

    private IEntity CreateZigzagBullet(SpectogramPoint point, Vector2 velocity, CircleProperties props)
    {
        var entity = new Entity();

        var transform = new TransformComponent { Position = point.Point };
        var render = new RenderComponent();
        render.SetSprite(_gameContext.GameEngine.SpriteFactory.CreateSprite(
            new CircleTexture(_gameContext.GameEngine.GraphicsDeviceManager.GraphicsDevice, props)));

        var zigzag = new ZigzagMoveComponent
        {
            BaseDirection = Vector2.Normalize(velocity),
            BaseSpeed = velocity.Length(),
            ZigzagAmplitude = 30f,
            ZigzagFrequency = 3f,
            CurrentTime = 0f,
            PerpendicularDirection = Vector2.Normalize(new Vector2(-velocity.Y, velocity.X))
        };

        entity.AddComponent(transform);
        entity.AddComponent(render);
        entity.AddComponent(zigzag);
        entity.AddComponent(new DestroyOnScreenBoundsComponent());

        return entity;
    }

    private IEntity CreatePulsingBullet(SpectogramPoint point, Vector2 velocity, CircleProperties props)
    {
        var entity = new Entity();

        var transform = new TransformComponent { Position = point.Point };
        var render = new RenderComponent();
        render.SetSprite(_gameContext.GameEngine.SpriteFactory.CreateSprite(
            new CircleTexture(_gameContext.GameEngine.GraphicsDeviceManager.GraphicsDevice, props)));

        var pulsing = new PulsingMoveComponent
        {
            Direction = Vector2.Normalize(velocity),
            BaseSpeed = velocity.Length(),
            PulseAmplitude = 0.5f,
            PulseFrequency = 4f,
            CurrentTime = 0f,
            BaseScale = 1.0f,
            ScalePulseAmplitude = 0.3f
        };

        entity.AddComponent(transform);
        entity.AddComponent(render);
        entity.AddComponent(pulsing);
        entity.AddComponent(new DestroyOnScreenBoundsComponent());

        return entity;
    }

    private IEntity CreateGravitationalBullet(SpectogramPoint point, Vector2 velocity, CircleProperties props, PlayerEntity? target)
    {
        var entity = new Entity();

        var transform = new TransformComponent { Position = point.Point };
        var render = new RenderComponent();
        render.SetSprite(_gameContext.GameEngine.SpriteFactory.CreateSprite(
            new CircleTexture(_gameContext.GameEngine.GraphicsDeviceManager.GraphicsDevice, props)));

        // Use player position as gravity center if available, otherwise use screen center
        Vector2 gravityCenter = Vector2.Zero;
        if (target.HasValue && target.Value.TryGetComponent<TransformComponent>(out var targetTransform))
        {
            gravityCenter = targetTransform.Position;
        }

        var gravitational = new GravitationalMoveComponent
        {
            Velocity = velocity,
            GravityCenter = gravityCenter,
            GravityStrength = 1000f,
            Mass = 1.0f,
            IsAttraction = true,
            MinDistance = 10f,
            MaxForce = 500f
        };

        entity.AddComponent(transform);
        entity.AddComponent(render);
        entity.AddComponent(gravitational);
        entity.AddComponent(new DestroyOnScreenBoundsComponent());

        return entity;
    }

    private IEntity CreateSeekingBullet(SpectogramPoint point, Vector2 velocity, CircleProperties props, PlayerEntity? target)
    {
        var entity = new Entity();

        var transform = new TransformComponent { Position = point.Point };
        var render = new RenderComponent();
        render.SetSprite(_gameContext.GameEngine.SpriteFactory.CreateSprite(
            new CircleTexture(_gameContext.GameEngine.GraphicsDeviceManager.GraphicsDevice, props)));

        Vector2 targetPosition = Vector2.Zero;
        bool hasTarget = false;
        if (target.HasValue && target.Value.TryGetComponent<TransformComponent>(out var targetTransform))
        {
            targetPosition = targetTransform.Position;
            hasTarget = true;
        }

        var seeking = new SeekingMoveComponent
        {
            Velocity = velocity,
            TargetPosition = targetPosition,
            MaxSpeed = velocity.Length() * 1.5f,
            SeekingForce = 5f,
            ArrivalRadius = 50f,
            PredictionTime = 0.5f,
            HasTarget = hasTarget,
            LastTargetUpdateTime = 0f,
            LastTargetPosition = targetPosition,
            PredictedTargetVelocity = Vector2.Zero
        };

        entity.AddComponent(transform);
        entity.AddComponent(render);
        entity.AddComponent(seeking);
        entity.AddComponent(new DestroyOnScreenBoundsComponent());

        return entity;
    }

    private IEntity CreateChainBullet(SpectogramPoint point, Vector2 velocity, CircleProperties props)
    {
        // Create chain head
        var entity = new Entity();

        var transform = new TransformComponent { Position = point.Point };
        var render = new RenderComponent();
        render.SetSprite(_gameContext.GameEngine.SpriteFactory.CreateSprite(
            new CircleTexture(_gameContext.GameEngine.GraphicsDeviceManager.GraphicsDevice, props)));

        var chain = new ChainMoveComponent
        {
            Velocity = velocity,
            ChainLinks = [],
            IsChainHead = true,
            ChainParent = null,
            ChainLinkDistance = 30f,
            ChainStiffness = 5f,
            ChainIndex = 0,
            LastPosition = point.Point
        };

        entity.AddComponent(transform);
        entity.AddComponent(render);
        entity.AddComponent(chain);
        entity.AddComponent(new DestroyOnScreenBoundsComponent());

        // Create chain links (3 additional segments)
        var currentParent = entity;
        for (int i = 1; i <= 3; i++)
        {
            var linkEntity = new Entity();
            var linkTransform = new TransformComponent
            {
                Position = point.Point - Vector2.Normalize(velocity) * i * 30f
            };
            var linkRender = new RenderComponent();

            // Make chain links smaller and different color
            var linkProps = new CircleProperties
            {
                Radius = props.Radius - 2,
                StrokeColor = props.StrokeColor,
                FillColor = Color.Lerp(props.FillColor, Color.Gray, 0.3f)
            };

            linkRender.SetSprite(_gameContext.GameEngine.SpriteFactory.CreateSprite(
                new CircleTexture(_gameContext.GameEngine.GraphicsDeviceManager.GraphicsDevice, linkProps)));

            var linkChain = new ChainMoveComponent
            {
                Velocity = Vector2.Zero,
                ChainLinks = [],
                IsChainHead = false,
                ChainParent = currentParent,
                ChainLinkDistance = 30f,
                ChainStiffness = 5f,
                ChainIndex = i,
                LastPosition = linkTransform.Position
            };

            linkEntity.AddComponent(linkTransform);
            linkEntity.AddComponent(linkRender);
            linkEntity.AddComponent(linkChain);
            linkEntity.AddComponent(new DestroyOnScreenBoundsComponent());

            // Add to chain links list
            if (currentParent.TryGetComponent<ChainMoveComponent>(out var parentChain))
            {
                parentChain.ChainLinks.Add(linkEntity);
            }

            currentParent = linkEntity;
        }

        return entity;
    }

    private BulletType DetermineBulletTypeFromAudio(MusicFeatures features)
    {
        // Enhanced audio feature to bullet type mapping
        var intensity = features.RMS;
        var frequency = features.Centroid;
        var spread = features.Spread;
        var beat = features.IsBeat;

        // Special patterns on beats
        if (beat)
        {
            if (intensity > 0.8f)
                return BulletType.Chain; // Explosive beats create chains
            else if (intensity > 0.6f)
                return BulletType.Cluster; // Strong beats create clusters
            else if (frequency > 0.7f)
                return BulletType.Chain; // High frequency beats create unpredictable bullets
            else
                return BulletType.Rocket; // Regular beats create homing bullets
        }

        // High frequency content
        if (frequency > 0.8f)
        {
            if (intensity > 0.6f)
                return BulletType.Laser; // Bright and loud = laser
            else
                return BulletType.Zigzag; // Bright but quiet = erratic movement
        }

        // High intensity (loud sections)
        if (intensity > 0.7f)
        {
            if (spread > 0.6f)
                return BulletType.Gravitational; // Wide and loud = gravity effects
            else
                return BulletType.Seeking; // Focused and loud = smart bullets
        }

        // Medium-high intensity
        if (intensity > 0.5f)
        {
            if (frequency > 0.5f)
                return BulletType.Pulsing; // Rhythmic sections
            else
                return BulletType.Wave; // Flowing movement
        }

        // Medium intensity with spread
        if (spread > 0.6f)
        {
            return BulletType.Bounce; // Wide frequency spread = chaotic bouncing
        }

        // Medium frequency content
        if (frequency > 0.4f)
        {
            if (intensity > 0.3f)
                return BulletType.Spiral; // Mid-range with some energy
            else
                return BulletType.Orbiting; // Gentle orbital movement
        }

        // Lower intensity patterns
        if (intensity > 0.3f)
        {
            return BulletType.Accelerating; // Building energy
        }
        else if (intensity > 0.2f)
        {
            return BulletType.Homing; // Gentle pursuit
        }
        else
        {
            return BulletType.Linear; // Default for quiet sections
        }
    }

    private Vector2 CalculateVelocityFromAudio(MusicFeatures features)
    {
        var baseSpeed = 1f + (features.RMS * 50f);
        var angle = features.Centroid * MathHelper.TwoPi;

        return new Vector2(
            (float)Math.Cos(angle) * baseSpeed,
            (float)Math.Sin(angle) * baseSpeed
        );
    }

    private Color GetColorFromAudio(MusicFeatures features)
    {
        // Map different features to color channels
        var r = (byte)Math.Clamp(features.Centroid * 255, 0, 255);
        var g = (byte)Math.Clamp(features.RMS * 255, 0, 255);
        var b = (byte)Math.Clamp(features.Spread * 255, 0, 255);

        return new Color(r, g, b);
    }

    private int GetSizeFromAudio(MusicFeatures features)
    {
        return (int)Math.Clamp(5 + (features.RMS * 15), 5, 20);
    }
}

public enum BulletType
{
    Linear,
    Homing,
    Spiral,
    Wave,
    Bounce,
    Rocket,
    Laser,
    Cluster,
    Accelerating,
    Orbiting,
    Zigzag,
    Pulsing,
    Gravitational,
    Seeking,
    Chain,
}

public struct MusicFeatures
{
    public float Centroid;
    public float RMS;
    public float Spread;
    public bool IsBeat;
}
