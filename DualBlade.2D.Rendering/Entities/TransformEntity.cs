using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Extensions;
using FunctionalMonads.Monads.MaybeMonad;

namespace DualBlade._2D.Rendering.Entities;

public class TransformEntity : Entity
{
    public TransformComponent Transform { get; init; }

    public TransformEntity()
    {
        Transform = AddComponent<TransformComponent>();
    }

    public IMaybe<IEntity> Parent =>
        Transform.Parent.Map(p => p.Entity);

    public IEnumerable<IEntity> Children
    {
        get => Transform.Children.Select(c => c.Entity);
        init => Transform.Children.AddRange(value.Select(x => x.GetComponent<INodeComponent>()).Somes());
    }
}
