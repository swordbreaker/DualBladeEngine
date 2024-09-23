using DualBlade._2D.Rendering.Components;
using DualBlade._2D.Rendering.Extensions;
using DualBlade.Core.Extensions;
using FluentAssertions;
using Microsoft.Xna.Framework;

namespace DualBlade._2D.Rendering.Tests.Extensions;

public class TransformComponentExtensionsTests
{
    [Fact()]
    public void AbsolutePositionTest()
    {
        // arrange
        var transformA = new TransformComponent() { Position = new Vector2(1, 2) };
        var transformB = new TransformComponent() { Position = new Vector2(2, 3) };

        transformB.AddParent(transformA);

        // act
        var absolutePosition = transformB.AbsolutePosition();

        // assert
        absolutePosition.Should().Be(new Vector2(3, 5));
    }

    [Fact()]
    public void AbsoluteScaleTest()
    {
        // arrange
        var transformA = new TransformComponent() { Scale = new Vector2(1, 2) };
        var transformB = new TransformComponent() { Scale = new Vector2(2, 3) };

        transformB.AddParent(transformA);

        // act
        var absoluteScale = transformB.AbsoluteScale();

        // assert
        absoluteScale.Should().Be(new Vector2(2, 6));
    }

    [Fact()]
    public void AbsoluteRotationTest()
    {
        // arrange
        var transformA = new TransformComponent() { Rotation = 1f };
        var transformB = new TransformComponent() { Rotation = 2f };

        transformB.AddParent(transformA);

        // act
        var absoluteRotation = transformB.AbsoluteRotation();

        // assert
        absoluteRotation.Should().Be(3f);
    }
}