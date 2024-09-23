using DualBlade.Core.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Extensions;
using DualBlade.Core.Factories;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;

namespace DualBlade.Core.Worlds;

public sealed class World(IGameEngine _gameEngine, ISystemFactory _systemFactory, IJobQueue _jobQueue) : IWorld
{
    private readonly HashSet<ISystem> _systems = [];
    private readonly Dictionary<Type, HashSet<IComponent>> _components = [];
    private readonly Dictionary<Type, HashSet<IEntity>> _entities = [];
    private bool isInitialized = false;

    public event Action<IEntity>? EntityAdded;
    public event Action<IEntity>? EntityDestroyed;
    public event Action<IComponent>? ComponentAdded;
    public event Action<IComponent>? ComponentDestroyed;

    public IEnumerable<IComponent> Components => _components.Values.SelectMany(x => x);
    public IEnumerable<ISystem> Systems => _systems;
    public IEnumerable<IEntity> Entities => _entities.Values.SelectMany(x => x);

    public void Initialize()
    {
        if (isInitialized)
        {
            throw new InvalidOperationException("World already initialized");
        }

        foreach (var system in _systems)
        {
            system.Initialize();
        }

        isInitialized = true;
    }

    public void AddSystems(params ISystem[] systems)
    {
        _systems.UnionWith(systems);

        if (isInitialized)
        {
            foreach (var system in systems)
            {
                system.Initialize();
            }
        }
    }

    public void AddSystem(ISystem system)
    {
        _systems.Add(system);

        if (isInitialized)
        {
            system.Initialize();
        }
    }

    public void AddEntities(params IEntity[] entities)
    {
        foreach (var entity in entities)
        {
            AddEntity(entity);
            entity.GetChildren().ToList().ForEach(AddEntity);
        }
    }

    private void AddComponent<TComponent>(TComponent component) where TComponent : IComponent
    {
        if (!_components.ContainsKey(component.GetType()))
        {
            _components[component.GetType()] = [];
        }

        _components[component.GetType()].Add(component);
        ComponentAdded?.Invoke(component);
    }

    public void AddEntity(IEntity entity)
    {
        if (!_entities.ContainsKey(entity.GetType()))
        {
            _entities[entity.GetType()] = [];
        }

        _entities[entity.GetType()].Add(entity);
        EntityAdded?.Invoke(entity);
        entity.GetChildren().ToList().ForEach(AddEntity);

        foreach (var component in entity.Components)
        {
            AddComponent(component);
        }
    }

    public void Destroy(IEntity entity)
    {
        // get the children before destroying the transform component
        var children = entity.GetChildren().ToList();

        foreach (var component in entity.Components)
        {
            DestroyComponent(component);
        }

        _entities[entity.GetType()].Remove(entity);
        EntityDestroyed?.Invoke(entity);

        children.ForEach(Destroy);
    }

    public void DestroyComponent(IComponent component)
    {
        if (!_entities.ContainsKey(component.Entity.GetType()) || !_entities[component.Entity.GetType()].TryGetValue(component.Entity, out var e))
        {
            throw new Exception("Entity not found in world");
        }

        if (!_components.ContainsKey(component.GetType()) || !_components[component.GetType()].TryGetValue(component, out var c))
        {
            throw new Exception("Component not found in world");
        }

        component.Entity.RemoveComponent(component.GetType());
        _components[component.GetType()].Remove(component);
        ComponentDestroyed?.Invoke(component);
    }

    public void DestroyComponent<TComponent>(IEntity entity) where TComponent : IComponent
    {
        entity.RemoveComponent<TComponent>();
        _components[typeof(TComponent)].Where(c => c.Entity == entity).ToList().ForEach(c => DestroyComponent(c));
    }

    public TComponent? GetComponent<TComponent>(IEntity entity) where TComponent : IComponent
    {
        if (!_entities.ContainsKey(entity.GetType()) || !_entities[entity.GetType()].Contains(entity))
        {
            throw new Exception("Entity not found in world");
        }

        if (!_components.ContainsKey(typeof(TComponent)))
        {
            return default;
        }

        return (TComponent?)_components[typeof(TComponent)]
                .FirstOrDefault(c => c.Entity == entity);
    }

    public IEnumerable<TComponent> GetComponents<TComponent>() where TComponent : IComponent
    {
        if (!_components.ContainsKey(typeof(TComponent)))
        {
            return [];
        }

        return _components[typeof(TComponent)].Cast<TComponent>();
    }

    public IEnumerable<TEntity> GetEntities<TEntity>() where TEntity : IEntity
    {
        if (!_entities.ContainsKey(typeof(TEntity)))
        {
            return [];
        }

        return _entities[typeof(TEntity)].Cast<TEntity>();
    }

    public void Update(GameTime gameTime)
    {
        _jobQueue.Execute();

        foreach (var system in _systems)
        {
            system.Update(gameTime);
        }
    }

    public void Draw(GameTime gameTime)
    {
        _gameEngine.BeginDraw();
        foreach (var system in _systems)
        {
            system.Draw(gameTime);
        }
        _gameEngine.EndDraw();
    }

    public void AddSystem<TSystem>() where TSystem : ISystem
    {
        AddSystem(_systemFactory.Create<TSystem>());
    }

    public void Destroy(ISystem system)
    {
        system.Dispose();
        _systems.Remove(system);
    }

    public void Destroy(IEnumerable<ISystem> systems)
    {
        foreach (var system in systems)
        {
            Destroy(system);
        }
    }
}
