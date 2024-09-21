using FunctionalMonads.Monads.MaybeMonad;
using System.Collections.Generic;

namespace MonoGameEngine.Engine.Components;
public interface INodeComponent : IComponent
{
    IMaybe<INodeComponent> Parent { get; set; }
    List<INodeComponent> Children { get; set; }
}
