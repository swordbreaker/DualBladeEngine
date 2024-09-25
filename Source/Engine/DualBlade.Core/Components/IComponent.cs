using DualBlade.Core.Entities;

namespace DualBlade.Core.Components;

public interface IComponent
{
    public IEntity Entity { init; get; }
}
