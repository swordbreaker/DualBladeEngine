using FunctionalMonads.Monads.MaybeMonad;

namespace DualBlade.Core.Components;
public interface INodeComponent : IComponent
{
    IMaybe<INodeComponent> Parent { get; set; }
    List<INodeComponent> Children { get; set; }
}
