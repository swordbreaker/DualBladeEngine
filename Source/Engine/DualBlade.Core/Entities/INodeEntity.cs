using DualBlade.Core.Components;

namespace DualBlade.Core.Entities
{
    public interface INodeEntity : IEntity
    {
        public INodeComponent NodeComponent { get; init; }

        IEnumerable<IEntity> Children { get; }
        INodeEntity? Parent { get; }

        void AddChild(INodeEntity child);
        void AddParent(INodeEntity parent);
    }
}
