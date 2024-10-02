using DualBlade.Core.Entities;

namespace DualBlade.Core.Worlds;
public partial class World
{
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

        foreach (var (system, _, start, end) in CollectComponentSystem(entity))
        {
            system.OnAdded(entity, entity.InternalComponents.ToSpan()[start..end], out var outEntity, out var outComponents);
            _entities[entity.Id] = outEntity;
            SyncEntityComponents(entity, outEntity, outComponents);
        }

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

        foreach (var (system, _, start, end) in CollectComponentSystem(entity))
        {
            system.OnDestroy(entity, entity.InternalComponents.ToSpan()[start..end]);
        }
    }
}
