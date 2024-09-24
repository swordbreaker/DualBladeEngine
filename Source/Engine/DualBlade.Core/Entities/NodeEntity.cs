using DualBlade.Core.Components;
using DualBlade.Core.Extensions;
using FunctionalMonads.Monads.MaybeMonad;

namespace DualBlade.Core.Entities
{
    public class NodeEntity : Entity, INodeEntity
    {
        public INodeComponent NodeComponent { get; init; }

        public NodeEntity()
        {
            NodeComponent = AddComponent<NodeComponent>();
        }

        public IMaybe<IEntity> Parent =>
            NodeComponent.Parent.Map(p => p.Entity);

        public IEnumerable<IEntity> Children
        {
            get => NodeComponent.Children.Select(c => c.Entity);
            init => NodeComponent.Children.AddRange(value.Select(x => x.GetComponent<INodeComponent>()).Somes());
        }

        public void AddChild(IEntity child)
        {
            child.GetComponent<INodeComponent>()
                .IfSome(NodeComponent.AddChild);
        }

        public void AddParent(IEntity parent)
        {
            parent.GetComponent<INodeComponent>()
                .IfSome(NodeComponent.AddParent);
        }
    }
}
