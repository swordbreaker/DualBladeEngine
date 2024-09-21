using FunctionalMonads.Monads.MaybeMonad;
using MonoGameEngine.Engine.Components;
using MonoGameEngine.Engine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameEngine.Engine.Extensions;

public static class EntityExtensions
{
    public static IMaybe<TComponent> GetComponent<TComponent>(this IEntity entity) where TComponent : IComponent
    {
        var components = entity.Components.OfType<TComponent>().ToArray();
        return components.Length switch
        {
            0 => Maybe.None<TComponent>(),
            1 => Maybe.Some(components[0]),
            _ => throw new Exception("Multiple components of the same type found")
        };
    }

    public static IMaybe<IEntity> GetParent(this IEntity entity) =>
        from thisTranform in entity.GetComponent<INodeComponent>()
        from parentTransform in thisTranform.Parent
        select parentTransform.Entity;

    public static IEnumerable<IEntity> GetChildren(this IEntity entity) =>
        entity.GetComponent<INodeComponent>()
            .Map(t => t.Children.Select(c => c.Entity))
            .SomeOrProvided([]);

    public static void AddChild(this Entity entity, Entity child)
    {
        var result =
            from parentTransform in entity.GetComponent<INodeComponent>()
            from childTransform in child.GetComponent<INodeComponent>()
            select (parentTransform, childTransform);

        result.IfSome(result => result.parentTransform.AddChild(result.childTransform));
    }

    public static void AddParent(this Entity entity, Entity parent)
    {
        var result =
            from childTransform in entity.GetComponent<INodeComponent>()
            from parentTransform in parent.GetComponent<INodeComponent>()
            select (childTransform, parentTransform);

        result.IfSome(result => result.childTransform.AddParent(result.parentTransform));
    }

    public static IMaybe<IEntity> Parent(this TransformEntity entity) =>
        entity.Transform.Parent.Map(p => p.Entity);

    public static IEnumerable<IEntity> Children(this TransformEntity entity) =>
        entity.Transform.Children.Select(c => c.Entity);
}
