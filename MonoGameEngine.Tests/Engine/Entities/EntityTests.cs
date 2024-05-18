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
        var entity = new Entity() { World = _world };
        entity.AddComponent(_component);

        entity.Components.Should().Contain(_component);
        var someComponent = entity.GetComponent<IComponent>();
        someComponent.IsSome.Should().BeTrue();
        someComponent.SomeOrProvided(Substitute.For<IComponent>()).Should().Be(_component);
    }

    [Fact()]
    public void RemoveComponentTest()
    {
        var entity = new Entity() { World = _world };
        entity.AddComponent(_component);

        entity.Components.Should().Contain(_component);
        entity.RemoveComponent<IComponent>();
        entity.Components.Should().NotContain(_component);
    }

    [Fact()]
    public void RemoveComponentTest1()
    {
        var entity = new Entity() { World = _world };
        entity.AddComponent(_component);

        entity.Components.Should().Contain(_component);
        entity.RemoveComponent(typeof(IComponent));
        entity.Components.Should().NotContain(_component);
    }

    [Fact()]
    public void AddComponentTest1()
    {
        var entity = new Entity() { World = _world };
        entity.AddComponent<TransformComponent>();

        var someComponent = entity.GetComponent<TransformComponent>();
        someComponent.IsSome.Should().BeTrue();
        someComponent.SomeOrProvided(Substitute.For<TransformComponent>()).Should().NotBeNull();
    }
}