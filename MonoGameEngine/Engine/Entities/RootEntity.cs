using FunctionalMonads.Monads.MaybeMonad;
using MonoGameEngine.Engine.Components;
using MonoGameEngine.Engine.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameEngine.Engine.Entities;

public class RootEntity : TransformEntity
{
    public required IEnumerable<IEntity> Childs
    {
        get => Transform.Children.Select(x => x.Entity);
        init => Transform.Children.AddRange(value.Select(x => x.GetComponent<TransformComponent>()).Somes());
    }
}
