using DualBlade._2D.BladePhysics.Components;
using DualBlade._2D.BladePhysics.Services;
using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Scenes;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using Example.PhysicsTest.Components;
using Example.PhysicsTest.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Example.PhysicsTest.Scences;

public class GravityScene : GameScene
{
    private readonly PhysicsSettings physicsSettings;

    public GravityScene(IGameContext context) : base(context)
    {
        physicsSettings = (PhysicsSettings)context.ServiceProvider.GetRequiredService<IPhysicsSettings>();
        physicsSettings.Gravity = new Vector2(0, -9.8f);
    }

    public override void Dispose()
    {
        base.Dispose();
        physicsSettings.Gravity = Vector2.Zero;
    }

    public override IEnumerable<ISystem> SetupSystems()
    {
        yield break;
    }

    protected override IEnumerable<EntityBuilder> SetupEntities()
    {
        var leftSquare = new PolySquareEntity(GameContext, new Vector2(-2, 0), Vector2.Zero);
        var rightSquare = new PolySquareEntity(GameContext, new Vector2(2, 0), Vector2.Zero);
        var ball = new CircleEntity(GameContext, new Vector2(-1, 0), new Vector2(0, 0));

        leftSquare.UpdateComponent<RigidBody>(r => r.SetIsStatic(true).SetCollectCollisionEvents(false));
        leftSquare.UpdateComponent<TransformComponent>(t =>
        {
            t.Rotation = -45;
            t.Scale = new(1, 3f);
            return t;
        });
        rightSquare.UpdateComponent<RigidBody>(r => r.SetIsStatic(true).SetCollectCollisionEvents(false));
        rightSquare.UpdateComponent<TransformComponent>(t =>
        {
            t.Rotation = 45;
            t.Scale = new(1, 3f);
            return t;
        });

        ball.RemoveComponent<ConstantVelocityComponent>();

        yield return CreateEntity(leftSquare);
        yield return CreateEntity(rightSquare);
        yield return CreateEntity(ball);
    }
}