using FunctionalMonads.Monads.MaybeMonad;
using MonoGameEngine.Engine.Components;

namespace MonoGameEngine.Engine.Extensions.Tests;

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

    [Fact()]
    public void AddChildTest()
    {
        // arrange
        var transformA = new TransformComponent();
        var transformB = new TransformComponent();

        // act
        transformA.AddChild(transformB);

        // assert
        transformA.Children.Should().Contain(transformB);
    }

    [Fact()]
    public void AddParentTest()
    {
        // arrange
        var transformA = new TransformComponent();
        var transformB = new TransformComponent();

        // act
        transformA.AddParent(transformB);

        // assert
        var parent = transformA.Parent.Should().BeAssignableTo<Some<TransformComponent>>().Subject.Value;
        parent.Should().Be(transformB);
    }
}