namespace DualBlade.Core.Components;

/// <summary>
/// Interface for a component, when implemented make sure to use a partial struct for the implementation.
/// </summary>
public interface IComponent
{
    /// <summary>
    /// Gets or sets the id of the component.
    /// </summary>
    int Id { get; set; }

    /// <summary>
    /// Gets or sets the id of the entity.
    /// </summary>
    int EntityId { get; set; }
}