using DualBlade.Core.Entities;

namespace DualBlade.Core.Components;
public struct BaseComponent : IComponent
{
    public IEntity Entity { get; init; }
}
