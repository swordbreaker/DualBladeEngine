using DualBlade.Core.Collections;
using DualBlade.Core.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Factories;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;

namespace DualBlade.Core.Worlds;

public sealed partial class World(ISystemFactory systemFactory, IJobQueue jobQueue) : IWorld
{
    public delegate ComponentRef<IComponent> AddComponentDelegate(IComponent component, int entityId);

    public delegate IComponent GetCopyDelegate(int id);

    public delegate ComponentProxy<IComponent> GetProxyDelegate<TComponent>();

    private readonly SparseCollection<IEntity> _entities = new(100);
    private readonly List<ISystem> _systems = new(100);
    private readonly List<FixedSystem> _fixedSystems = new();
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

        foreach (var system in _activeComponentSystems)
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

        //Parallel.ForEach(_componentSystemData.ToSpan().ToArray(), (tuple, state, count) =>
        //{
        //    var (system, entity, start, end) = tuple;
        //    system.Update(entity, entity.InternalComponents.ToSpan()[start..end], gameTime, out var outEntity, out var outComponents);
        //    _entities[entity.Id] = outEntity;
        //    SyncEntityComponents(entity, outEntity, outComponents);
        //});

        foreach (var (system, entity, start, end) in _componentSystemData.ToSpan())
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

        foreach (var system in _activeComponentSystems)
        {
            system.LateUpdate(gameTime);
        }

        // 15 FPS
        if (gameTime.TotalGameTime.Subtract(lastFixedUpdate).TotalMilliseconds > 66)
        {
            foreach (var (system, entity, start, end) in _componentSystemData.ToSpan())
            {
                system.FixedUpdate(entity, entity.InternalComponents.ToSpan()[start..end], gameTime, out var outEntity,
                    out var outComponents);
                _entities[entity.Id] = outEntity;
                SyncEntityComponents(entity, outEntity, outComponents);
            }

            foreach (var fixedSystem in _fixedSystems)
            {
                fixedSystem.Update(new GameTime(gameTime.TotalGameTime,
                    gameTime.TotalGameTime.Subtract(lastFixedUpdate)));
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
        foreach (var system in _activeComponentSystems)
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
        foreach (var (system, entity, start, end) in _componentSystemData.ToSpan())
        {
            system.Draw(entity, entity.InternalComponents.ToSpan()[start..end], gameTime);
        }

        // Late draw
        foreach (var system in _activeComponentSystems)
        {
            system.LateDraw(gameTime);
        }

        foreach (var system in _activeEntitySystems)
        {
            system.LateDraw(gameTime);
        }
    }
}