using DualBlade.Core.Collections;
using DualBlade.Core.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Factories;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;

namespace DualBlade.Core.Worlds;

public sealed class World(ISystemFactory systemFactory, IJobQueue jobQueue) : IWorld
{
    public delegate ComponentRef<IComponent> AddComponentDelegate(IComponent component);
    public delegate IComponent GetCopyDelegate(int id);
    public delegate ComponentProxy<IComponent> GetProxyDelegate<TComponent>();

    private readonly SparseCollection<IEntity> _entities = new(100);
    private readonly SparseCollection<IComponent> _components = new(10000);
    private readonly List<ISystem> _systems = new(100);
    private readonly Dictionary<Type, List<IComponentSystem>> _componentSystems = [];
    private readonly Dictionary<Type, List<IEntitySystem>> _entitySystems = [];

    private bool isInitialized = false;

    public IEnumerable<IComponent> Components => _components.Values();
    public IEnumerable<ISystem> Systems => _systems;
    public IEnumerable<IEntity> Entities => _entities.Values();

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

    #region Systems
    public void AddSystems(params ISystem[] systems)
    {
        foreach (var system in systems)
        {
            AddSystem(system);
        }
    }

    public void AddSystem<TSystem>() where TSystem : ISystem
    {
        AddSystem(systemFactory.Create<TSystem>());
    }

    public void AddSystem(ISystem system)
    {
        if (!_systems.Contains(system))
        {
            switch (system)
            {
                case IComponentSystem componentSystem:
                    if (!_componentSystems.ContainsKey(componentSystem.CompType))
                    {
                        _componentSystems[componentSystem.CompType] = [];
                    }

                    _componentSystems[componentSystem.CompType].Add(componentSystem);
                    break;
                case IEntitySystem entitySystem:
                    if (!_entitySystems.ContainsKey(entitySystem.EntityType))
                    {
                        _entitySystems[entitySystem.EntityType] = [];
                    }

                    _entitySystems[entitySystem.EntityType].Add(entitySystem);
                    break;
                default:
                    _systems.Add(system);
                    break;
            }

            if (isInitialized)
            {
                system.Initialize();
            }
        }
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
    #endregion

    #region Entity
    public void AddEntities(params IEntity[] entities)
    {
        foreach (var entity in entities)
        {
            AddEntity(entity);
        }
    }

    public void AddEntity<TEntity>(TEntity entity) where TEntity : IEntity
    {
        // TODO add child entities
        if (entity is INodeEntity nodeEntity)
        {
            foreach (var child in nodeEntity.Children)
            {
                AddEntity(child);
            }
        }

        var id = _entities.NextFreeIndex();
        entity.Init(AddComponent, id);
        _entities.Add(entity);

        foreach (var component in entity.InitialComponents.ToSpan())
        {
            entity.Components.Add(AddComponent(component));
        }

        if (_entitySystems.TryGetValue(typeof(TEntity), out var systems))
        {
            foreach (var system in systems)
            {
                _entities[entity.Id] = system.OnAdded(entity);
            }
        }
    }

    public EntityProxy<TEntity> GetEntityProxy<TEntity>(int id) where TEntity : IEntity =>
        new(UpdateIEntity, (TEntity)_entities[id], id);

    public void UpdateIEntity(IEntity entity)
    {
        _entities[entity.Id] = entity;
    }

    public void Destroy(IEntity entity)
    {
        var nodeComponentRef = entity.Component<INodeComponent>();

        if (nodeComponentRef.HasValue)
        {
            var nodeComponent = nodeComponentRef.Value.GetCopy();

            // get the children before destroying the transform component
            var children = new List<IEntity>(nodeComponent.Children.Length);
            foreach (var child in nodeComponent.Children.ToSpan())
            {
                children.Add(_entities[child.GetCopy().EntityId]);
            }

            children.ForEach(Destroy);
        }

        foreach (var component in entity.Components.ToSpan())
        {
            Destroy(component.GetCopy());
        }

        if (_entitySystems.TryGetValue(entity.GetType(), out var systems))
        {
            foreach (var system in systems)
            {
                system.OnDestroy(entity);
            }
        }
    }

    #endregion

    #region Components

    private ComponentRef<TComponent> AddComponent<TComponent>(TComponent component) where TComponent : IComponent
    {
        component.Id = _components.NextFreeIndex();
        _components.Add(component);

        if (_componentSystems.TryGetValue(typeof(TComponent), out var systems))
        {
            foreach (var system in systems)
            {
                _components[component.Id] = system.OnAdded(component);
            }
        }

        return new ComponentRef<TComponent>(this, component.Id);
    }

    ComponentRef<TComponent> IWorld.AddComponent<TComponent>(TComponent component) =>
        AddComponent(component);

    private TComponent GetComponentCopy<TComponent>(int id) where TComponent : IComponent =>
        (TComponent)_components[id];
    TComponent IWorld.GetComponentCopy<TComponent>(int id) => this.GetComponentCopy<TComponent>(id);

    private ComponentProxy<TComponent> GetComponentProxy<TComponent>(int id) where TComponent : IComponent
    {
        var component = (TComponent)_components[id];
        return new ComponentProxy<TComponent>(UpdateComponent, component, id);
    }

    ComponentProxy<TComponent> IWorld.GetComponentProxy<TComponent>(int id) => GetComponentProxy<TComponent>(id);

    private ComponentRef<TComponent> GetComponent<TComponent>(int id) where TComponent : IComponent =>
        new(this, id);

    ComponentRef<TComponent> IWorld.GetComponent<TComponent>(int id) =>
        GetComponent<TComponent>(id);

    public void UpdateComponent(IComponent component)
    {
        _components[component.Id] = component;
    }

    private void Destroy(IComponent component)
    {
        _components.Remove(component.Id);

        if (_componentSystems.TryGetValue(component.GetType(), out var systems))
        {
            foreach (var system in systems)
            {
                system.OnDestroy(component);
            }
        }
    }

    void IWorld.Destroy(IComponent component) => Destroy(component);
    #endregion

    public void Update(GameTime gameTime)
    {
        jobQueue.Execute();

        foreach (var system in _systems)
        {
            system.Update(gameTime);
        }

        foreach (var component in _components.Values())
        {
            if (_componentSystems.TryGetValue(component.GetType(), out var systems))
            {
                foreach (var system in systems)
                {
                    system.Update(gameTime);
                    _components[component.Id] = system.Update(component, gameTime);
                }
            }
        }

        foreach (var entity in _entities.Values())
        {
            if (_entitySystems.TryGetValue(entity.GetType(), out var systems))
            {
                foreach (var system in systems)
                {
                    system.Update(gameTime);
                    _entities[entity.Id] = system.Update(entity, gameTime);
                }
            }
        }
    }

    public void Draw(GameTime gameTime)
    {
        foreach (var system in _systems)
        {
            system.Draw(gameTime);
        }

        foreach (var component in _components.Values())
        {
            if (_componentSystems.TryGetValue(component.GetType(), out var systems))
            {
                foreach (var system in systems)
                {
                    system.Draw(gameTime);
                    system.Draw(component, gameTime);
                }
            }
        }

        foreach (var entity in _entities.Values())
        {
            if (_entitySystems.TryGetValue(entity.GetType(), out var systems))
            {
                foreach (var system in systems)
                {
                    system.Draw(gameTime);
                    system.Draw(entity, gameTime);
                }
            }
        }
    }
}
