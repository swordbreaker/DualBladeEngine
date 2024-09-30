using DualBlade.Core.Collections;
using DualBlade.Core.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Factories;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using DualBlade.Core.Utils;
using System.Diagnostics;

namespace DualBlade.Core.Worlds;

public sealed class World(ISystemFactory systemFactory, IJobQueue jobQueue) : IWorld
{
    private const int MaxComponentTypesOnComponentSystem = 5;

    public delegate ComponentRef<IComponent> AddComponentDelegate(IComponent component, int entityId);
    public delegate IComponent GetCopyDelegate(int id);
    public delegate ComponentProxy<IComponent> GetProxyDelegate<TComponent>();

    private delegate void UpdateFunc(IComponentSystem system, Span<IComponent> components);

    private readonly SparseCollection<IEntity> _entities = new(100);
    private readonly List<ISystem> _systems = new(100);
    private readonly Dictionary<Type, List<IComponentSystem>> _componentSystems = [];
    private readonly Dictionary<Type, List<IEntitySystem>> _entitySystems = [];

    private bool isInitialized = false;

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
                    if (!_componentSystems.ContainsKey(componentSystem.CompTypes.Span[0]))
                    {
                        _componentSystems[componentSystem.CompTypes.Span[0]] = [];
                    }

                    _componentSystems[componentSystem.CompTypes.Span[0]].Add(componentSystem);
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

    public TEntity AddEntity<TEntity>(TEntity entity) where TEntity : IEntity
    {
        var id = _entities.NextFreeIndex();
        entity.Init(id);
        _entities.Add(entity);

        if (_entitySystems.TryGetValue(typeof(TEntity), out var systems))
        {
            foreach (var system in systems)
            {
                _entities[entity.Id] = system.OnAdded(entity);
            }
        }

        UpdateComponentSystems(entity, (system, components) =>
        {
            system.OnAdded(entity, components, out var outEntity, out var outComponents);
            _entities[entity.Id] = outEntity;
            SyncEntityComponents(entity, outEntity, outComponents);
        });

        return (TEntity)_entities[id];
    }

    public EntityProxy<TEntity> GetEntityProxy<TEntity>(int id) where TEntity : IEntity =>
        new(UpdateEntity, (TEntity)_entities[id], id);

    public void UpdateEntity(IEntity entity)
    {
        _entities[entity.Id] = entity;
    }

    public void Destroy(IEntity entity)
    {
        foreach (var childId in entity.Children.ToSpan())
        {
            if (childId >= 0)
            {
                Destroy(_entities[childId]);
            }
        }

        _entities.Remove(entity.Id);

        if (_entitySystems.TryGetValue(entity.GetType(), out var systems))
        {
            foreach (var system in systems)
            {
                system.OnDestroy(entity);
            }
        }

        UpdateComponentSystems(entity, (system, components) =>
        {
            system.OnDestroy(entity, components);
        });
    }

    #endregion

    private unsafe void UpdateComponentSystems(IEntity entity, UpdateFunc updateFunc)
    {
        for (int i = 0; i < entity.ComponentTypes.Length; i++)
        {
            if (_componentSystems.TryGetValue(entity.ComponentTypes.Span[i], out var componentSystems))
            {
                UpdateComponentSystemsForEntity(entity, updateFunc, componentSystems, i);
            }
        }

        //if (entity.ComponentTypes.Length > 0 && _componentSystems.TryGetValue(entity.ComponentTypes.Span[0], out var componentSystems))
        //{
        //    UpdateComponentSystemsForEntity(entity, updateFunc, componentSystems);
        //}
    }

    private static unsafe void UpdateComponentSystemsForEntity(IEntity entity, UpdateFunc updateFunc, List<IComponentSystem> componentSystems, int entityStartPointer)
    {
        foreach (var system in componentSystems)
        {
            ProcessComponentSystem(entity, updateFunc, entityStartPointer, system);
        }
    }

    private static unsafe void ProcessComponentSystem(IEntity entity, UpdateFunc updateFunc, int entityStartPointer, IComponentSystem system)
    {
        int i = 1;
        var entityPointer = entityStartPointer + 1;
        for (; i < system.CompTypes.Length; i++)
        {
            // cancel if one type does not match, because both types are ordered we know that the rest will not match
            if (entity.ComponentTypes.Span[entityPointer] != system.CompTypes.Span[i])
            {
                return;
            }
            entityPointer++;
        }

        var entityEndPointer = entityPointer;
        var len = i;
        if (len < system.CompTypes.Length)
        {
            return;
        }

        updateFunc(system, entity.InternalComponents.ToSpan()[entityStartPointer..entityEndPointer]);
    }

    private void SyncEntityComponents(IEntity entity, IEntity outEntity, Span<IComponent> components)
    {
        _entities[entity.Id] = outEntity;
        foreach (var newComp in components)
        {
            for (int i = 0; i < _entities[entity.Id].InternalComponents.Length; i++)
            {
                if (newComp.GetType() == _entities[entity.Id].InternalComponents[i].GetType())
                {
                    _entities[entity.Id].InternalComponents[i] = newComp;
                }
            }
        }
    }

    public void Update(GameTime gameTime)
    {
        jobQueue.Execute();

        foreach (var system in _systems)
        {
            system.Update(gameTime);
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

            UpdateComponentSystems(entity, (system, components) =>
            {
                system.Update(gameTime);
                system.Update(entity, components, gameTime, out var outEntity, out var outComponents);
                _entities[entity.Id] = outEntity;

                SyncEntityComponents(entity, outEntity, outComponents);
            });
        }
    }

    public void Draw(GameTime gameTime)
    {
        foreach (var system in _systems)
        {
            system.Draw(gameTime);
        }

        foreach (var entity in _entities.Values())
        {
            if (_entitySystems.TryGetValue(entity.GetType(), out var systems))
            {
                foreach (var system in systems)
                {
                    system.Draw(gameTime);
                    system.Draw(entity, gameTime);
                    system.AfterDraw(gameTime);
                }
            }

            UpdateComponentSystems(entity, (system, components) =>
            {
                system.Draw(gameTime);
                system.Draw(entity, components, gameTime);
                system.AfterDraw(gameTime);
            });
        }
    }
}
