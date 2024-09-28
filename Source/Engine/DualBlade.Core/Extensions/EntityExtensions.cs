using DualBlade.Core.Components;
using DualBlade.Core.Entities;

namespace DualBlade.Core.Extensions;

public static class EntityExtensions
{
    //public static TComponent? GetComponent<TComponent>(this IEntity entity) where TComponent : IComponent
    //{
    //    var components = entity.Components.OfType<TComponent>().ToArray();
    //    return components.Length switch
    //    {
    //        0 => default,
    //        1 => components[0],
    //        _ => throw new Exception("Multiple components of the same type found")
    //    };
    //}

    //public static IEntity? GetParent(this IEntity entity)
    //{
    //    var thisNode = entity.GetComponent<INodeComponent>();
    //    var parentNode = thisNode?.Parent;
    //    return parentNode?.Entity;
    //}

    //public static IEnumerable<IEntity> GetChildren(this IEntity entity)
    //{
    //    var node = entity.GetComponent<INodeComponent>();
    //    if (node is null)
    //    {
    //        return [];
    //    }

    //    return node.Children.Select(c => c.Entity);
    //}

    //public static void AddChild(this Entity entity, Entity child)
    //{
    //    var parentTransform = entity.GetComponent<INodeComponent>();
    //    var childTransform = child.GetComponent<INodeComponent>();

    //    if (parentTransform is null || childTransform is null)
    //    {
    //        return;
    //    }

    //    parentTransform.AddChild(childTransform);
    //}

    //public static void AddParent(this Entity entity, Entity parent)
    //{
    //    var childTransform = entity.GetComponent<INodeComponent>();
    //    var parentTransform = parent.GetComponent<INodeComponent>();

    //    if (childTransform is null || parentTransform is null)
    //    {
    //        return;
    //    }

    //    childTransform.AddParent(parentTransform);
    //}

    //public static IEntity? Parent(this INodeEntity entity) =>
    //    entity.NodeComponent.Parent.Entity;

    //public static IEnumerable<IEntity> Children(this INodeEntity entity) =>
    //    entity.NodeComponent.Children.Select(c => c.Entity);
}
