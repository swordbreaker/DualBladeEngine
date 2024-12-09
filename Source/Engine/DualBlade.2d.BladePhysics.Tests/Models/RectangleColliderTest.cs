using DualBlade._2D.BladePhysics.Models;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;

namespace TestProject1.Models;

[TestSubject(typeof(RectangleCollider))]
public class RectangleColliderTest
{
    //[Fact]
    //public void METHOD()
    //{
    //    // arrange
    //    var rectA = new RectangleCollider(Vector2.Zero, Vector2.One);
    //    var rectB = new RectangleCollider(Vector2.One, Vector2.One);

    //    // act
    //    var isColliding = rectA.HitTest(rectB, out var info);

    //    // assert
    //    var expectedNormal = Vector2.Normalize(new Vector2(-1, -1));

    //    isColliding.Should().BeTrue();
    //    info.Normal.X.Should().BeApproximately(expectedNormal.X, 0.1f);
    //    info.Normal.Y.Should().BeApproximately(expectedNormal.Y, 0.1f);

    //    info.ContactPoint.X.Should().BeApproximately(0.5f, 0.1f);
    //    info.ContactPoint.Y.Should().BeApproximately(0.5f, 0.1f);
    //}

    [Theory]
    [InlineData(1, 0, -1, 0)]
    [InlineData(-1, 0, 1, 0)]
    [InlineData(0, 1, 0, -1)]
    [InlineData(1, 1, -0.7, -0.7)]
    public void NormalTest(float cx, float cy, float nx, float ny)
    {
        // arrange
        var rectA = new RectangleCollider(Vector2.Zero, Vector2.One);
        var rectB = new RectangleCollider(new Vector2(cx, cy), Vector2.One);

        var expectedNormal = new Vector2(nx, ny);

        // act
        var isColliding = rectA.HitTest(rectB, out var info);

        // assert
        isColliding.Should().BeTrue();
        info.Normal.X.Should().BeApproximately(expectedNormal.X, 0.1f);
        info.Normal.Y.Should().BeApproximately(expectedNormal.Y, 0.1f);
    }

    [Theory]
    [InlineData(1, 0, 0.5, 0)]
    [InlineData(-1, 0, -0.5, 0)]
    [InlineData(0, 1, 0, 0.5)]
    [InlineData(1, 1, 0.5, 0.5)]
    public void ContactPointTest(float cx, float cy, float x, float y)
    {
        // arrange
        var rectA = new RectangleCollider(Vector2.Zero, Vector2.One);
        var rectB = new RectangleCollider(new Vector2(cx, cy), Vector2.One);

        var expectedContactPoint = new Vector2(x, y);

        // act
        var isColliding = rectA.HitTest(rectB, out var info);

        // assert
        isColliding.Should().BeTrue();
        info.ContactPoint.X.Should().BeApproximately(expectedContactPoint.X, 0.1f);
        info.ContactPoint.Y.Should().BeApproximately(expectedContactPoint.Y, 0.1f);
    }
}