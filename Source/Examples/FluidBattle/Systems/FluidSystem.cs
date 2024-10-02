using DualBlade._2D.Physics.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Extensions;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using DualBlade.Core.Utils;
using FluidBattle.Components;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Common;
using System;
using static Microsoft.Xna.Framework.Vector2;

namespace FluidBattle.Systems;
public class FluidSystem : ComponentSystem<KinematicComponent, FluidComponent>
{
    static Random random = new Random();
    private static PerlinNoise2D perlinNoise = new(42);

    private static float FlowStrength = 5f;
    private static float NoiseStrength = 1f;
    private float maxDistance;

    public FluidSystem(IGameContext gameContext) : base(gameContext)
    {
        maxDistance = MathF.Max(gameContext.GameEngine.GameSize.X, gameContext.GameEngine.GameSize.Y);
    }

    protected override void Update(ref KinematicComponent kinematic, ref FluidComponent fluid, ref IEntity entity, GameTime gameTime)
    {
        var pos = kinematic.PhysicsBody.Position;
        var time = (float)gameTime.TotalGameTime.TotalSeconds;

        var generalFlow = Normalize(fluid.Target - pos) * FlowStrength;

        var noise = new Vector2(
            perlinNoise.Noise(pos.X, time),
            perlinNoise.Noise(pos.Y, time))
            * NoiseStrength;

        float distanceToTarget = Distance(pos, fluid.Target);
        float speedFactor = MathUtils.Clamp(1 - distanceToTarget / maxDistance, 0, 1);

        kinematic.PhysicsBody.LinearVelocity = (generalFlow + noise) * speedFactor;
    }
}
