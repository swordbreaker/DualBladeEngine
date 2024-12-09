using DualBlade._2D.BladePhysics.Models;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;

namespace TestProject1.Models;

[TestSubject(typeof(CircleCollider))]
public class CircleColliderTest
{
    [Fact]
    public void HitTestOnCircleReturnsTrueAndTheCorrectInfoOnCollision()
    {
        // arrange
        var circle1 = new CircleCollider()
        {
            Radius = 0.6f,
            Center = Vector2.Zero,
        };

        var circle2 = new CircleCollider()
        {
            Radius = 0.6f,
            Center = new Vector2(1f, 0)
        };

        // act
        var isColliding = circle1.HitTest(circle2, out var info);

        // assert
        isColliding.Should().BeTrue();
        info.Collider.Should().Be(circle1);
        info.Normal.Should().Be(new Vector2(-1, 0));
        info.ContactPoint.Should().Be(new Vector2(0.6f, 0));
    }

    [Fact]
    public void HitTestOnCircleReturnsFalseAndOnNonCollision()
    {
        // arrange
        var circle1 = new CircleCollider()
        {
            Radius = 0.6f,
            Center = Vector2.Zero,
        };

        var circle2 = new CircleCollider()
        {
            Radius = 0.6f,
            Center = new Vector2(2f, 0)
        };

        // act
        var isColliding = circle1.HitTest(circle2, out var _);

        // assert
        isColliding.Should().BeFalse();
    }

    [Fact]
    public void HitTestOnRectangleReturnsTrueAndCorrectInfoOnCollision()
    {
        // arrange
        var circle1 = new CircleCollider(Vector2.Zero, 0.8f);
        var rectangle = new RectangleCollider(new Vector2(1, 0), Vector2.One * 0.6f);

        // act
        var isColliding = circle1.HitTest(rectangle, out var info);

        // assert
        isColliding.Should().BeTrue();
        info.Collider.Should().Be(circle1);
        info.Normal.Should().Be(new Vector2(-1, 0));
        info.ContactPoint.Should().Be(new Vector2(0.7f, 0));
    }
}