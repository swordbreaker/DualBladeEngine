﻿using DualBlade._2D.BladePhysics.Models;
using DualBlade.Core.Components;

namespace DualBlade._2D.BladePhysics.Components;
public partial struct RigidBody : IComponent
{
    public Vector2 Velocity;
    public Vector2 Acceleration;
    public float Mass;

    public bool CollectCollisionEvents;
}