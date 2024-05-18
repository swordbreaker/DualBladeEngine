using FluentAssertions;
using FunctionalMonads.Monads.MaybeMonad;
using MonoGameEngine.Engine.Components;
using MonoGameEngine.Engine.Extensions;
using MonoGameEngine.Engine.Worlds;
using NSubstitute;

namespace MonoGameEngine.Engine.Entities.Tests;

public class EntityTests
{
    private readonly IWorld _world;
    private readonly IComponent _component;

    public EntityTests()
    {
        _world = Substitute.For<IWorld>();
        _component = Substitute.For<IComponent>();
    }

    [Fact()]
    public void AddComponentTest()
    {
        // arrange
        var entity = new Entity() { World = _world };
        
        // act
        entity.AddComponent(_component);

        // assert
        entity.Components.Should().Contain(_component);
        var someComponent = entity.GetComponent<IComponent>();
        someComponent.IsSome.Should().BeTrue();
        someComponent.SomeOrProvided(Substitute.For<IComponent>()).Should().Be(_component);
    }

    [Fact()]
    public void RemoveComponentTest()
    {
        // arrange
        var entity = new Entity() { World = _world };
        
        // act
        entity.AddComponent(_component);

        // assert
        entity.Components.Should().Contain(_component);
        entity.RemoveComponent<IComponent>();
        entity.Components.Should().NotContain(_component);
    }

    [Fact()]
    public void RemoveComponentTest1()
    {
        // arrange
        var entity = new Entity() { World = _world };
        
        // act
        entity.AddComponent(_component);

        // assert
        entity.Components.Should().Contain(_component);
        entity.RemoveComponent(typeof(IComponent));
        entity.Components.Should().NotContain(_component);
    }

    [Fact()]
    public void AddComponentTest1()
    {
        // arrange 
        var entity = new Entity() { World = _world };
        
        // act
        entity.AddComponent<TransformComponent>();

        // assert
        var someComponent = entity.GetComponent<TransformComponent>();
        someComponent.IsSome.Should().BeTrue();
        someComponent.SomeOrProvided(Substitute.For<TransformComponent>()).Should().NotBeNull();
    }

    [Fact()]
    public void AddComponentWithFactoryTest()
    {
        // arrange & act
        var entity = new DummyEntity(e => new TransformComponent() { Entity = e }) { World = _world };

        // assert
        var transformComponent = entity.GetComponent<TransformComponent>().Should().BeAssignableTo<Some<TransformComponent>>().Subject.Value;
        transformComponent.Entity.Should().Be(entity);
    }

    public class DummyEntity : Entity
    {
        public DummyEntity(Func<IEntity, IComponent> factory)
        {
            this.AddComponent(factory);
        }
    }
}