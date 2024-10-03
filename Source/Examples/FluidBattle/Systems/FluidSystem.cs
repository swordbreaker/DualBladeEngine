using DualBlade._2D.Physics.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using DualBlade.Core.Utils;
using FluidBattle.Components;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Common;
using System;
using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Dynamics.Contacts;
using static Microsoft.Xna.Framework.Vector2;
using FluidBattle.Entities;
using DualBlade._2D.Rendering.Components;
using System.Diagnostics.Metrics;

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

    protected override void OnAdded(ref IEntity entity, ref KinematicComponent kinematic, ref FluidComponent _)
    {
        kinematic.PhysicsBody.OnCollision += PhysicsBody_OnCollision;
    }

    protected override void OnDestroy(KinematicComponent component, FluidComponent component2, IEntity entity)
    {
        component.PhysicsBody.OnCollision -= PhysicsBody_OnCollision;
    }

    private bool PhysicsBody_OnCollision(Fixture sender, Fixture other, Contact contact)
    {
        if (sender.Tag is not FluidEntity thisFluid || other.Tag is not FluidEntity otherFluid)
        {
            return true;
        }

        using var thisFluidComp = thisFluid.FluidComponentProxy;
        using var otherFluidComp = otherFluid.FluidComponentProxy;

        if (thisFluidComp.Value.Player == otherFluidComp.Value.Player)
        {
            return true;
        }

        var thisSpeed = sender.Body.LinearVelocity.LengthSquared();
        var otherSpeed = other.Body.LinearVelocity.LengthSquared();

        var (thisP, otherP) = Softmax(thisSpeed, otherSpeed);

        var t = random.NextDouble();

        if (t < thisP)
        {
            TransformOther(ref thisFluidComp.Value, ref otherFluidComp.Value, otherFluid);
        }
        else
        {
            TransformOther(ref otherFluidComp.Value, ref thisFluidComp.Value, thisFluid);
        }

        return true;
    }

    private static (float, float) Softmax(float a, float b)
    {
        // Subtract the max value for numerical stability
        float max = MathF.Max(a, b);
        float expA = MathF.Exp(a - max);
        float expB = MathF.Exp(b - max);

        float sum = expA + expB;

        return (expA / sum, expB / sum);
    }

    private void TransformOther(ref FluidComponent thisFluid, ref FluidComponent otherFluid, IEntity otherEntity)
    {
        otherFluid.Player = thisFluid.Player;
        foreach (var child in Ecs.GetChildren(otherEntity))
        {
            using var childRenderComp = child.Component<RenderComponent>();
            childRenderComp.Value.Color = thisFluid.Color;
        }
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
