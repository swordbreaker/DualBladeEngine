using System;
using System.Collections.Generic;
using TestMonoGamesProject.Engine.World;
using IComponent = TestMonoGamesProject.Engine.Components.IComponent;

namespace TestMonoGamesProject.Engine.Entities
{
    public interface IEntity
    {
        IWorld World { init; get; }
        IEnumerable<IComponent> Components { get; }
        TComponent AddComponent<TComponent>() where TComponent : IComponent, new();
        void AddComponent<TComponent>(TComponent component) where TComponent : IComponent;
        void RemoveComponent<TComponent>() where TComponent : IComponent;
        void RemoveComponent(Type type);
    }
}
