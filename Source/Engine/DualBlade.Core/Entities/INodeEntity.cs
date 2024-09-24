using DualBlade.Core.Components;
using FunctionalMonads.Monads.MaybeMonad;

namespace DualBlade.Core.Entities
{
    public interface INodeEntity : IEntity
    {
        public INodeComponent NodeComponent { get; init; }

        IEnumerable<IEntity> Children { get; }
        IMaybe<IEntity> Parent { get; }

        void AddChild(IEntity child);
        void AddParent(IEntity parent);
    }
}
