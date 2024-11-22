using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using DualBlade.Core.Utils;
using FluidBattle.Components;
using FluidBattle.Entities;
using DualBlade._2D.Rendering.Components;
using DualBlade._2D.BladePhysics.Components;
using DualBlade._2D.Rendering.Extensions;
using DualBlade._2D.BladePhysics.Models;
using DualBlade._2D.BladePhysics.Services;
using System.Linq;

namespace FluidBattle.Systems;

public class FluidSystem : ComponentSystem<RigidBody, FluidComponent>
{
    private readonly IJobQueue jobQueue;
    private readonly IPhysicsManager physicsManager;

    static Random random = new Random();
    private static PerlinNoise2D perlinNoise = new(42);

    private static float FlowStrength = 5f;
    private static float NoiseStrength = 1f;
    private float maxDistance;

    public FluidSystem(IGameContext gameContext) : base(gameContext)
    {
        maxDistance = MathF.Max(gameContext.GameEngine.GameSize.X, gameContext.GameEngine.GameSize.Y);
        jobQueue = GetService<IJobQueue>();
        physicsManager = GetService<IPhysicsManager>();
    }

    protected override void OnAdded(ref IEntity entity, ref RigidBody body, ref FluidComponent fluid)
    {
        if (entity.TryGetComponent<ColliderComponent>(out var colliderComponent))
        {
            foreach (var collider in colliderComponent.Colliders)
            {
                collider.Tag = entity;
            }
        }
    }

    private bool OnCollision(FluidEntity thisEntity, FluidComponent thisFluidComp, RigidBody thisBody, CollisionInfo info)
    {
        if (info.Collider.Tag is not FluidEntity otherEntity)
        {
            return true;
        }

        var otherFluidComp = otherEntity.FluidComponentCopy;
        var otherBody = otherEntity.RigidBodyCopy;
        var thisTransform = thisEntity.TransformComponentCopy;
        var otherTransform = otherEntity.TransformComponentCopy;

        if (thisFluidComp.Player == otherFluidComp.Player)
        {
            return true;
        }

        float thisP;
        if (thisFluidComp.Player < 0)
        {
            thisP = 0;
        }
        else if (otherFluidComp.Player < 0)
        {
            thisP = 1;
        }
        else
        {
            var center = otherTransform.Position + (thisTransform.Position - otherTransform.Position) / 2;
            var thisToCenter = center - thisTransform.Position;
            var otherToCenter = center - otherTransform.Position;

            var thisDot = Dot(thisToCenter, thisBody.Velocity);
            var otherDot = Dot(otherToCenter, otherBody.Velocity);
            (thisP, _) = Softmax(thisDot, otherDot);
        }

        var t = random.NextSingle();

        if (t < thisP)
        {
            TransformOther(thisFluidComp, otherFluidComp, otherEntity);
        }
        else
        {
            TransformOther(otherFluidComp, thisFluidComp, thisEntity);
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

    protected override void Update(ref RigidBody body, ref FluidComponent fluid, ref IEntity entity, GameTime gameTime)
    {
        if (fluid.Player < 0)
        {
            return;
        }

        var pos = Ecs.AbsolutePosition(entity);
        var time = (float)gameTime.TotalGameTime.TotalSeconds;

        var generalFlow = Normalize(fluid.Target - pos) * FlowStrength;

        var noise = new Vector2(
            perlinNoise.Noise(pos.X, time),
            perlinNoise.Noise(pos.Y, time))
            * NoiseStrength;

        float distanceToTarget = Distance(pos, fluid.Target);

        float speedFactor = Math.Clamp(1 - distanceToTarget / maxDistance, 0, 1);

        body.Velocity = (generalFlow + noise) * speedFactor;

        foreach (var info in physicsManager.GetNewCollisions(body).Where(x => x.Collider.Tag is FluidEntity))
        {
            OnCollision((FluidEntity)entity, fluid, body, info);
        }
    }
}
