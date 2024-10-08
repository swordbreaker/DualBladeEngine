﻿using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using Microsoft.Xna.Framework;
using PerformanceTest.Components;

namespace PerformanceTest.Entities;

[AddComponent<ParticleEmitterComponent>]
public partial struct ParticleEmitterEntity : IEntity
{
    public ParticleEmitterEntity(Vector2 pos)
    {
        AddComponent(new TransformComponent() { Position = pos });
    }
}
