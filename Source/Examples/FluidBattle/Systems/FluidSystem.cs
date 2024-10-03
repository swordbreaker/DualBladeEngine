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
using FluidBattle.Entities;
using DualBlade._2D.Rendering.Components;

namespace FluidBattle.Systems;

public class FluidSystem : ComponentSystem<KinematicComponent, FluidComponent>
{
    private readonly IJobQueue jobQueue;

    static Random random = new Random();
    private static PerlinNoise2D perlinNoise = new(42);

    private static float FlowStrength = 5f;
    private static float NoiseStrength = 1f;
    private float maxDistance;

    public FluidSystem(IGameContext gameContext) : base(gameContext)
    {
        maxDistance = MathF.Max(gameContext.GameEngine.GameSize.X, gameContext.GameEngine.GameSize.Y);
        jobQueue = GetService<IJobQueue>();
    }

    protected override void OnAdded(ref IEntity entity, ref KinematicComponent kinematic, ref FluidComponent _)
    {
        kinematic.PhysicsBody.Tag = entity;
        foreach (var fixture in kinematic.PhysicsBody.FixtureList)
        {
            fixture.Tag = entity;
        }

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

        var thisFluidComp = thisFluid.FluidComponentCopy;
        var otherFluidComp = otherFluid.FluidComponentCopy;

        if (thisFluidComp.Player == otherFluidComp.Player)
        {
            return true;
        }

        var center = other.Body.Position + (sender.Body.Position - other.Body.Position) / 2;
        var thisToCenter = center - sender.Body.Position;
        var otherToCenter = center - other.Body.Position;

        var thisDot = Dot(thisToCenter, sender.Body.LinearVelocity);
        var otherDot = Dot(otherToCenter, other.Body.LinearVelocity);

        var (thisP, otherP) = Softmax(thisDot, otherDot);

        var t = random.NextDouble();

        if (t < thisP)
        {
            TransformOther(thisFluidComp, otherFluidComp, otherFluid);
        }
        else
        {
            TransformOther(otherFluidComp, thisFluidComp, thisFluid);
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

    private void TransformOther(FluidComponent thisFluid, FluidComponent otherFluid, IEntity otherEntity)
    {
        otherFluid.Player = thisFluid.Player;
        otherFluid.Color = thisFluid.Color;

        foreach (var child in Ecs.GetChildren(otherEntity))
        {
            var entity = child.GetCopy();
            var childRenderComp = entity.Component<RenderComponent>();
            childRenderComp.Color = thisFluid.Color;
            Ecs.UpdateComponent(entity, childRenderComp);
        }

        Ecs.UpdateComponent(otherEntity, otherFluid);
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
