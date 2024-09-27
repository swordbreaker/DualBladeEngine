using DualBlade.Core.Entities;

namespace DualBlade.Core.Components;
public class ComponentBase : IComponent
{
    public IEntity Entity { get; init; }
}
