using DualBlade.Core.Components;

namespace DualBlade.Core.Entities
{
    public interface INodeEntity : IEntity
    {
        public INodeComponent NodeComponent { get; init; }

        IEnumerable<IEntity> Children { get; }
        IEntity? Parent { get; }

        void AddChild(IEntity child);
        void AddParent(IEntity parent);
    }
}
