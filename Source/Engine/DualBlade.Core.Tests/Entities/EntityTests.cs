using DualBlade.Core.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Extensions;
using DualBlade.Core.Worlds;
using FunctionalMonads.Monads.MaybeMonad;

namespace DualBlade.Core.Tests.Entities;

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
        var entity = new Entity();

        // act
        entity.AddComponent(_component);

        // assert
        entity.Components.Should().Contain(_component);
        var someComponent = entity.GetComponent<IComponent>();
        someComponent.Should().Be(_component);
    }

    [Fact()]
    public void RemoveComponentTest()
    {
        // arrange
        var entity = new Entity();

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
        var entity = new Entity();

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
        var entity = new Entity();

        // act
        entity.AddComponent<NodeComponent>();

        // assert
        var someComponent = entity.GetComponent<NodeComponent>();
        someComponent.Should().NotBeNull();
    }

    [Fact()]
    public void AddComponentWithFactoryTest()
    {
        // arrange & act
        var entity = new DummyEntity(e => new NodeComponent() { Entity = e });

        // assert
        var transformComponent = entity.GetComponent<NodeComponent>().Should().BeAssignableTo<Some<NodeComponent>>().Subject.Value;
        //transformComponent.Entity.Should().Be(entity);
    }

    public class DummyEntity : Entity
    {
        public DummyEntity(Func<IEntity, IComponent> factory)
        {
            //AddComponent(factory());
        }
    }
}