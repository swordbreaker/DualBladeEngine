using DualBlade.Core.Collections;
using DualBlade.Core.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Factories;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;

namespace DualBlade.Core.Worlds;

/// <inheritdoc cref="IWorld"/>
public sealed partial class World(ISystemFactory systemFactory, IJobQueue jobQueue) : IWorld
{
    public delegate ComponentRef<IComponent> AddComponentDelegate(IComponent component, int entityId);

    public delegate IComponent GetCopyDelegate(int id);

    public delegate ComponentProxy<IComponent> GetProxyDelegate<TComponent>();

    private readonly SparseCollection<IEntity> _entities = new(100);
    private readonly List<ISystem> _systems = new(100);
    private readonly Dictionary<Type, List<IComponentSystem>> _componentSystems = [];
    private readonly Dictionary<Type, List<IEntitySystem>> _entitySystems = [];

    private bool isInitialized = false;

    private TimeSpan lastFixedUpdate = TimeSpan.Zero;

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

    public void Update(GameTime gameTime)
    {
        jobQueue.Execute();

        foreach (var system in _systems)
        {
            system.Update(gameTime);
        }

        // collect and update
        CollectComponentSystems();
        CollectEntitySystems();

        foreach (var system in activeComponentSystems)
        {
            system.Update(gameTime);
        }

        foreach (var system in _activeEntitySystems)
        {
            system.Update(gameTime);
        }

        // update for entities
        foreach (var (system, entity) in _entitySystemsData.ToSpan())
        {
            _entities[entity.Id] = system.Update(entity, gameTime);
        }

        foreach (var (system, entity, start, end) in componentSystemData.ToSpan())
        {
            system.Update(entity, entity.InternalComponents.ToSpan()[start..end], gameTime, out var outEntity,
                out var outComponents);
            _entities[entity.Id] = outEntity;
            SyncEntityComponents(entity, outEntity, outComponents);
        }

        // Late draw
        foreach (var system in _activeEntitySystems)
        {
            system.LateUpdate(gameTime);
        }

        foreach (var system in activeComponentSystems)
        {
            system.LateUpdate(gameTime);
        }

        // 30 FPS
        if (gameTime.TotalGameTime.Subtract(lastFixedUpdate).TotalMilliseconds > 33)
        {
            foreach (var (system, entity, start, end) in componentSystemData.ToSpan())
            {
                system.FixedUpdate(entity, entity.InternalComponents.ToSpan()[start..end], gameTime, out var outEntity,
                    out var outComponents);
                _entities[entity.Id] = outEntity;
                SyncEntityComponents(entity, outEntity, outComponents);
            }

            lastFixedUpdate = gameTime.TotalGameTime;
        }
    }

    public void Draw(GameTime gameTime)
    {
        foreach (var system in _systems)
        {
            system.Draw(gameTime);
        }

        // collect and draw
        CollectComponentSystems();
        CollectEntitySystems();

        // draw foreach system
        foreach (var system in activeComponentSystems)
        {
            system.Draw(gameTime);
        }

        foreach (var system in _activeEntitySystems)
        {
            system.Draw(gameTime);
        }

        // draw for entities
        foreach (var (system, entity) in _entitySystemsData.ToSpan())
        {
            system.Draw(entity, gameTime);
        }

        // draw foreach component in component system
        foreach (var (system, entity, start, end) in componentSystemData.ToSpan())
        {
            system.Draw(entity, entity.InternalComponents.ToSpan()[start..end], gameTime);
        }

        // Late draw
        foreach (var system in activeComponentSystems)
        {
            system.LateDraw(gameTime);
        }

        foreach (var system in _activeEntitySystems)
        {
            system.LateDraw(gameTime);
        }
    }
}