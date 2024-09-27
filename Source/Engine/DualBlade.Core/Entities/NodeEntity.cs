//using DualBlade.Core.Components;
//using DualBlade.Core.Extensions;

//namespace DualBlade.Core.Entities
//{
//    public class NodeEntity : Entity, INodeEntity
//    {
//        public INodeComponent NodeComponent { get; init; }

//        public NodeEntity()
//        {
//            NodeComponent = AddComponent<NodeComponent>();
//        }

//        public IEntity? Parent =>
//            NodeComponent.Parent?.Entity;

//        public IEnumerable<IEntity> Children
//        {
//            get => NodeComponent.Children.Select(c => c.Entity);
//            init => NodeComponent.Children.AddRange(value.Select(x => x.GetComponent<INodeComponent>()).OfType<INodeComponent>());
//        }

//        public void AddChild(IEntity child)
//        {
//            if (child.GetComponent<INodeComponent>() is INodeComponent node)
//            {
//                NodeComponent.AddChild(node);
//            }
//        }

//        public void AddParent(IEntity parent)
//        {
//            if (parent.GetComponent<INodeComponent>() is INodeComponent node)
//            {
//                NodeComponent.AddParent(node);
//            }
//        }
//    }
//}
