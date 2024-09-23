using DualBlade.Core.Entities;
using FunctionalMonads.Monads.MaybeMonad;

namespace DualBlade.Core.Components;

public class NodeComponent : INodeComponent
{
    public IMaybe<INodeComponent> Parent { get; set; } = Maybe.None<INodeComponent>();
    public List<INodeComponent> Children { get; set; } = [];
    public IEntity Entity { get; init; }
}
