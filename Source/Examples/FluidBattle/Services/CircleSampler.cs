using DualBlade.Core.Extensions;
using Microsoft.Xna.Framework;
using System;

namespace FluidBattle.Services;

public interface ICircleSampler
{
    Vector2 Sample(float minRadius, float maxRadius, float minAngle = 0, float maxAngle = MathHelper.TwoPi);
}

public class CircleSampler : ICircleSampler
{
    private static readonly Random Random = new();

    public Vector2 Sample(float minRadius, float maxRadius, float minAngle = 0, float maxAngle = MathHelper.TwoPi)
    {
        var radius = Random.NextFloat(minRadius, maxRadius);
        var angle = Random.NextFloat(0, MathHelper.TwoPi);
        return new Vector2(MathF.Cos(angle), MathF.Sin(angle)) * radius;
    }
}
