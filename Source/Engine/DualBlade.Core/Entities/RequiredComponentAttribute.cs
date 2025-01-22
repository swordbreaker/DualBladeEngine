namespace DualBlade.Core.Entities;

/// <summary>
/// Attribute to mark a struct as a required component. This will generate methods in a component to access the required component.
/// </summary>
/// <typeparam name="TComponent"></typeparam>
[AttributeUsage(AttributeTargets.Struct, AllowMultiple = true)]
public sealed class RequiredComponentAttribute<TComponent> : Attribute
{
}