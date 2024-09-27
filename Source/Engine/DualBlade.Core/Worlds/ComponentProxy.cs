namespace DualBlade.Core.Worlds;

/// <summary>
/// Proxy for a component that allows for mutation.
/// This is a ref struct to avoid heap allocations.
/// This copies the value and stores it for manipulation, and then updates the original value when disposed.
/// 
/// You cannot store this in a field or return it from a method.
/// Only use this within a method scope.
/// </summary>
/// <typeparam name="T"></typeparam>
public unsafe ref struct ComponentProxy<T> where T : ITestComponent
{
    private readonly int id;
    private readonly Action<int, ITestComponent> updateAction;
    private T value;

    internal ComponentProxy(Action<int, ITestComponent> updateAction, T value, int id)
    {
        this.updateAction = updateAction;
        this.id = id;
        this.value = value;
    }

    public ComponentProxy<TOther> As<TOther>() where TOther : T =>
        new(updateAction, (TOther)value, id);

    public ref T Value => ref value;

    public void Dispose() => updateAction(id, value);
}
