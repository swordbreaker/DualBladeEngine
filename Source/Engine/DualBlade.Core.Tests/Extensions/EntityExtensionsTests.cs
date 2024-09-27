//using FunctionalMonads.Monads.MaybeMonad;
//using DualBlade.Core.Entities;
//using DualBlade.Core.Components;
//using DualBlade.Core.Extensions;

//namespace DualBlade.Core.Tests.Extensions;

//public class EntityExtensionsTests
//{
//    private readonly IEntity _entity;

//    public EntityExtensionsTests()
//    {
//        _entity = Substitute.For<IEntity>();
//    }

//    [Fact()]
//    public void GetComponentTest()
//    {
//        // arrange
//        var expectedComponent = Substitute.For<IComponent>();
//        _entity.Components.Returns([expectedComponent]);

//        // act
//        var component = _entity.GetComponent<IComponent>();

//        // assert
//        component.Should().Be(expectedComponent);
//    }

//    [Fact()]
//    public void GetParentTest()
//    {
//        // arrange
//        var parentEntity = new NodeEntity();
//        var childEntity = new NodeEntity();

//        parentEntity.AddChild(childEntity);

//        // act
//        var maybeParten = childEntity.GetParent();

//        // assert
//        var parent = maybeParten.Should().BeAssignableTo<Some<IEntity>>().Subject.Value;
//        parent.Should().Be(parentEntity);
//    }

//    [Fact()]
//    public void GetChildrenTest()
//    {
//        // arrange
//        var parentComponent = new NodeComponent() { Entity = Substitute.For<IEntity>() };
//        var childComponent = new NodeComponent() { Entity = Substitute.For<IEntity>() };
//        parentComponent.AddChild(childComponent);

//        var parentEntity = new NodeEntity();
//        var childEntity = new NodeEntity();

//        parentEntity.AddChild(childEntity);

//        // act
//        var children = parentEntity.GetChildren();

//        // assert
//        children.Should().Contain(childEntity);
//    }

//    [Fact()]
//    public void AddChildTest()
//    {
//        // arrange
//        var parentEntity = new NodeEntity();
//        var childEntity = new NodeEntity();

//        // act
//        parentEntity.AddChild(childEntity);

//        // assert
//        parentEntity.GetChildren().Should().Contain(childEntity);
//    }

//    [Fact()]
//    public void AddParentTest()
//    {
//        // arrange
//        var parentEntity = new NodeEntity();
//        var childEntity = new NodeEntity();

//        // act
//        childEntity.AddParent(parentEntity);

//        // assert
//        var parent = childEntity.GetParent().Should().BeAssignableTo<Some<IEntity>>().Subject.Value;
//        parent.Should().Be(parentEntity);
//    }

//    [Fact()]
//    public void ParentTest()
//    {
//        // arrange
//        var parentEntity = new NodeEntity();
//        var childEntity = new NodeEntity();

//        childEntity.AddParent(parentEntity);

//        // act
//        var maybeeParent = childEntity.Parent();

//        // assert
//        var parent = maybeeParent.Should().BeAssignableTo<Some<IEntity>>().Subject.Value;
//        parent.Should().Be(parentEntity);

//    }

//    [Fact()]
//    public void ChildrenTest()
//    {
//        // arrange
//        var parentEntity = new NodeEntity();
//        var childEntity = new NodeEntity();

//        parentEntity.AddChild(childEntity);

//        // act
//        var children = parentEntity.Children();

//        // assert
//        children.Should().Contain(childEntity);
//    }
//}