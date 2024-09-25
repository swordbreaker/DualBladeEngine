using DualBlade.Core.Components;
using DualBlade.Core.Entities;
using Microsoft.Xna.Framework;
using System.Runtime.CompilerServices;

namespace DualBlade.Core.Worlds.Tests;

public struct TestComp : IComponent
{
    public IEntity Entity { get; init; }

    public int MyNumber;
    public string MyString;
}

public class TestSystem : ICompSys
{
    public Type ComponentType => typeof(TestComp);

    public IComponent LastComp;

    public void Update(ref IComponent component, GameTime gameTime)
    {
        LastComp = component;

        var testComp = Unsafe.As<IComponent, TestComp>(ref component);
        testComp.MyNumber = 5;
        component = testComp;
    }

    public void Update(GameTime gameTime) { }
    public void Draw(GameTime gameTime) { }
    public void Initialize() { }
    public void Dispose() { }
}

public class TestWorldTests
{
    [Fact()]
    public void UpdateTest()
    {
        var world = new TestWorld();
        var entity = new Entity();

        var refComp = world.AddComponent(new TestComp { MyNumber = 2, MyString = "Test", Entity = entity });
        ref var cc = ref refComp.Value;
        cc.MyNumber = 3;

        var comp = world.GetComponent<TestComp>(entity);

        comp.MyNumber.Should().Be(3);

        var sys = new TestSystem();
        world.AddSystem(sys);
        world.Update(new GameTime());

        var c = sys.LastComp.Should().BeAssignableTo<TestComp>().Subject;
        c.MyNumber.Should().Be(2);

        world.Update(new GameTime());
        c = sys.LastComp.Should().BeAssignableTo<TestComp>().Subject;
        c.MyNumber.Should().Be(5);
    }
}