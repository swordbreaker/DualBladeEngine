using DualBlade.Core.Components;
using DualBlade.Core.Extensions;
using FunctionalMonads.Monads.MaybeMonad;

namespace DualBlade.Core.Tests.Extensions;

public class NodeComponentExtensionsTests
{
    [Fact()]
    public void AddChildTest()
    {
        // arrange
        var transformA = new NodeComponent();
        var transformB = new NodeComponent();

        // act
        //transformA.AddChild(transformB);

        //// assert
        //transformA.Children.Should().Contain(transformB);
    }

    [Fact()]
    public void AddParentTest()
    {
        // arrange
        var transformA = new NodeComponent();
        var transformB = new NodeComponent();

        // act
        //transformA.AddParent(transformB);

        //// assert
        //var parent = transformA.Parent.Should().BeAssignableTo<Some<INodeComponent>>().Subject.Value;
        //parent.Should().Be(transformB);
    }
}