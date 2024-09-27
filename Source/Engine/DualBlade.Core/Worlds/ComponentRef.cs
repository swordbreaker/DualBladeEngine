namespace DualBlade.Core.Worlds;

/// <summary>
/// Holds a reference to a component that allows for reading.
/// 
/// Use the <see cref="GetCopy"/> method to get a copy of the component.
/// Use the <see cref="GetProxy"/> method to get a proxy for the component that allows for mutation.
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly struct ComponentRef<T> where T : ITestComponent
{
    private readonly TestWorld testWorld;
    private readonly int id;

    internal ComponentRef(TestWorld testWorld, int id)
    {
        this.testWorld = testWorld;
        this.id = id;
    }

    internal ComponentRef<TOther> As<TOther>() where TOther : ITestComponent =>
        new(testWorld, id);

    /// <summary>
    /// Gets a copy of the component.
    /// </summary>s
    public T GetCopy() => testWorld.GetComponent<T>(id);

    /// <summary>
    /// Gets a proxy for the component that allows for mutation.
    /// </summary>
    public ComponentProxy<T> GetProxy() => testWorld.GetComponentRef<T>(id);
}
