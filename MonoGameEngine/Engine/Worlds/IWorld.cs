using MonoGameEngine.Engine.Components;
using MonoGameEngine.Engine.Entities;
using MonoGameEngine.Engine.Systems;
using System;
using System.Collections.Generic;

namespace MonoGameEngine.Engine.Worlds;

public interface IWorld
{
    IEnumerable<IComponent> Components { get; }
    IEnumerable<ISystem> Systems { get; }
    IEnumerable<IEntity> Entities { get; }

    event Action<IEntity> EntityAdded;
    event Action<IEntity> EntityDestroyed;
    event Action<IComponent> ComponentAdded;
    event Action<IComponent> ComponentDestroyed;

    void Update(GameTime gameTime);
    void Draw(GameTime gameTime);

    void Destroy(IEntity entity);
    void DestroyComponent(IComponent component);

    void Destroy(ISystem system);
    void Destroy(IEnumerable<ISystem> systems);

    void AddEntity(IEntity entity);
    IEnumerable<TComponent> GetComponents<TComponent>() where TComponent : IComponent;
    TComponent? GetComponent<TComponent>(IEntity entity) where TComponent : IComponent;

    IEnumerable<TEntity> GetEntities<TEntity>() where TEntity : IEntity;

    void AddSystems(params ISystem[] systems);
    void AddSystem(ISystem system);
    void AddSystem<TSystem>() where TSystem : ISystemWithWorld, new();
    void DestroyComponent<TComponent>(IEntity entity) where TComponent : IComponent;
    void Initialize();
}
