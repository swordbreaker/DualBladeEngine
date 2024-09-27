using Microsoft.Xna.Framework;
using System.Diagnostics;
using Xunit.Abstractions;

namespace DualBlade.Core.Worlds.Tests;

public struct CompA : ITestComponent
{
    public int Value;

    public int Id { get; set; }
    public int EntityId { get; set; }
}

public struct CompB : ITestComponent
{
    public int Value;
    public Vector2 Position;

    public int Id { get; set; }
    public int EntityId { get; set; }
}

public struct LifetimeComponent : ITestComponent
{
    public int LifeCount;

    public int Id { get; set; }
    public int EntityId { get; set; }
}

public class LifetimeSystem(TestWorld world) : TestSystem<LifetimeComponent>
{
    protected override void Update(ref LifetimeComponent component)
    {
        if (component.LifeCount > 5)
        {
            world.RemoveComponent(component);
        }
        component.LifeCount++;
    }
}

public class SpawnerSystem(TestWorld world) : TestSystem<LifetimeComponent>
{
    public override void Update()
    {
        world.AddComponent(new LifetimeComponent());
    }

    protected override void Update(ref LifetimeComponent component) { }
}

public class MySystem : TestSystem<CompA>
{
    protected override void Update(ref CompA component)
    {
        component.Value = 42;
    }
}

public class TestWorldTests(ITestOutputHelper _output)
{
    private TestWorld _world;

    [Fact()]
    public void UpdateTest()
    {
        _world = new TestWorld();
        _world.AddComponent(new CompA());

        using (var compRef = _world.GetComponentRef<CompA>(0))
        {
            compRef.Value.Value = 42;
        }

        _world.GetComponent<CompA>(0).Value.Should().Be(42);
    }

    [Fact]
    public void Test2()
    {
        var world = new TestWorld();

        world.AddComponent(new CompA());
        world._systems.Add(new MySystem());

        world.Update();

        world.GetComponent<CompA>(0).Value.Should().Be(42);
    }

    [Fact]
    public void Test3()
    {
        int[] initialCounts = new int[GC.MaxGeneration + 1];

        // Store initial counts
        for (int i = 0; i <= GC.MaxGeneration; i++)
        {
            initialCounts[i] = GC.CollectionCount(i);
        }

        var world = new TestWorld();

        world._systems.Add(new SpawnerSystem(world));
        world._systems.Add(new LifetimeSystem(world));

        for (var i = 0; i < 100; i++)
        {
            world.Update();
            _output.WriteLine(world._components.Count.ToString());
        }

        GC.Collect();

        // Check for changes
        for (int i = 0; i <= GC.MaxGeneration; i++)
        {
            int currentCount = GC.CollectionCount(i);
            int collectionsSinceStart = currentCount - initialCounts[i];
            _output.WriteLine($"Generation {i} collections since start: {collectionsSinceStart}");
        }
    }
}