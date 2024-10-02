﻿using DualBlade._2D.Physics.Components;
using DualBlade._2D.Physics.Services;
using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using FluidBattle.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;

namespace FluidBattle.Entities;

[AddComponent<FluidComponent>]
public partial struct FluidEntity : IEntity
{
    public FluidEntity(IGameContext context, Vector2 position, float radius, float scale)
    {
        var physicsManager = context.ServiceProvider.GetRequiredService<IPhysicsManager>();
        var worldToPixel = context.GameEngine.WorldToPixelConverter;

        var transform = new TransformComponent
        {
            Position = position
        };

        //var pixelRadius = radius / worldToPixel.TileSize;

        // Create a physics body
        var body = physicsManager.CreateBody(transform.Position, bodyType: BodyType.Dynamic);
        body.IgnoreGravity = true;
        body.FixedRotation = true;
        body.Mass = 1;
        body.LinearDamping = 0f;
        var fixture = body.CreateCircle(radius, 1);
        fixture.Restitution = 0.01f;
        fixture.Friction = 0f;

        var kinematic = new KinematicComponent
        {
            PhysicsBody = body
        };

        AddComponent(transform);
        AddComponent(kinematic);
    }
}
