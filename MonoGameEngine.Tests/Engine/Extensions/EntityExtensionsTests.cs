﻿using MonoGameEngine.Engine.Entities;
using MonoGameEngine.Engine.Components;
using FunctionalMonads.Monads.MaybeMonad;
using MonoGameEngine.Engine.Worlds;

namespace MonoGameEngine.Engine.Extensions.Tests;

public class EntityExtensionsTests
{
    private readonly IEntity _entity;

    public EntityExtensionsTests()
    {
        _entity = Substitute.For<IEntity>();
    }

    [Fact()]
    public void GetComponentTest()
    {
        // arrange
        var expectedComponent = Substitute.For<IComponent>();
        _entity.Components.Returns(new List<IComponent> { expectedComponent });

        // act
        var component = _entity.GetComponent<IComponent>();

        // assert
        component.IsSome.Should().BeTrue();
        if (component is Some<IComponent> someComponent)
        {
            someComponent.Value.Should().Be(expectedComponent);
        }
    }

    [Fact()]
    public void GetParentTest()
    {
        // arrange
        var parentEntity = new TransformEntity { World = Substitute.For<IWorld>() };
        var childEntity = new TransformEntity { World = Substitute.For<IWorld>() };

        parentEntity.AddChild(childEntity);

        // act
        var maybeParten = childEntity.GetParent();

        // assert
        var parent = maybeParten.Should().BeAssignableTo<Some<IEntity>>().Subject.Value;
        parent.Should().Be(parentEntity);
    }

    [Fact()]
    public void GetChildrenTest()
    {
        // arrange
        var parentComponent = new TransformComponent() { Entity = Substitute.For<IEntity>() };
        var childComponent = new TransformComponent() { Entity = Substitute.For<IEntity>() };
        parentComponent.AddChild(childComponent);

        var parentEntity = new TransformEntity { World = Substitute.For<IWorld>() };
        var childEntity = new TransformEntity { World = Substitute.For<IWorld>() };

        parentEntity.AddChild(childEntity);

        // act
        var children = parentEntity.GetChildren();

        // assert
        children.Should().Contain(childEntity);
    }

    [Fact()]
    public void AddChildTest()
    {
        // arrange
        var parentEntity = new TransformEntity { World = Substitute.For<IWorld>() };
        var childEntity = new TransformEntity { World = Substitute.For<IWorld>() };

        // act
        parentEntity.AddChild(childEntity);

        // assert
        parentEntity.GetChildren().Should().Contain(childEntity);
    }

    [Fact()]
    public void AddParentTest()
    {
        // arrange
        var parentEntity = new TransformEntity { World = Substitute.For<IWorld>() };
        var childEntity = new TransformEntity { World = Substitute.For<IWorld>() };

        // act
        childEntity.AddParent(parentEntity);

        // assert
        var parent = childEntity.GetParent().Should().BeAssignableTo<Some<IEntity>>().Subject.Value;
        parent.Should().Be(parentEntity);
    }

    [Fact()]
    public void ParentTest()
    {
        // arrange
        var parentEntity = new TransformEntity { World = Substitute.For<IWorld>() };
        var childEntity = new TransformEntity { World = Substitute.For<IWorld>() };

        childEntity.AddParent(parentEntity);

        // act
        var maybeeParent = childEntity.Parent();

        // assert
        var parent = maybeeParent.Should().BeAssignableTo<Some<IEntity>>().Subject.Value;
        parent.Should().Be(parentEntity);

    }

    [Fact()]
    public void ChildrenTest()
    {
        // arrange
        var parentEntity = new TransformEntity { World = Substitute.For<IWorld>() };
        var childEntity = new TransformEntity { World = Substitute.For<IWorld>() };

        parentEntity.AddChild(childEntity);

        // act
        var children = parentEntity.Children();

        // assert
        children.Should().Contain(childEntity);
    }
}