using FunctionalMonads.Monads.MaybeMonad;
using MonoGameEngine.Engine.Entities;
using System.Collections.Generic;

namespace MonoGameEngine.Engine.Components;

public class NodeComponent : INodeComponent
{
    public IMaybe<INodeComponent> Parent { get; set; } = Maybe.None<INodeComponent>();
    public List<INodeComponent> Children { get; set; } = [];
    public IEntity Entity { get; init; }
}
