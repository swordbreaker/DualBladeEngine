namespace DualBlade.Core.Entities;

/// <summary>
/// Used to add a component to an entity.
/// </summary>
/// <typeparam name="TComponent">The type of the component to add.</typeparam>
[AttributeUsage(AttributeTargets.Struct, AllowMultiple = true)]
public sealed class AddComponentAttribute<TComponent> : Attribute
{
}