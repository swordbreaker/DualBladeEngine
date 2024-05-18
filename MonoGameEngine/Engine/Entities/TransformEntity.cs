using FunctionalMonads.Monads.MaybeMonad;
using MonoGameEngine.Engine.Components;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameEngine.Engine.Entities;

public class TransformEntity : Entity
{
    public TransformComponent Transform { get; init; }

    public TransformEntity()
    {
        Transform = AddComponent<TransformComponent>();
    }

    public IMaybe<IEntity> Parent =>
        Transform.Parent.Map(p => p.Entity);

    public IEnumerable<IEntity> Children =>
        Transform.Children.Select(c => c.Entity);
}
