namespace DualBlade.Core.Entities;

[AttributeUsage(AttributeTargets.Struct, AllowMultiple = true)]
public class AddComponentAttribute<TComponent> : Attribute
{
}
