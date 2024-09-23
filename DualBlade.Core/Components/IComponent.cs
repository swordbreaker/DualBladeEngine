using DualBlade.Core.Entities;

namespace DualBlade.Core.Components;

public interface IComponent
{
    IEntity Entity { init; get; }
}
