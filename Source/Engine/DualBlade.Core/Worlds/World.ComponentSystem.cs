using DualBlade.Core.Collections;
using DualBlade.Core.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Systems;

namespace DualBlade.Core.Worlds;

public partial class World
{
    private readonly GrowableMemory<(IComponentSystem system, IEntity entity, int start, int end)>
        componentSystemData = new(100);

    private readonly HashSet<IComponentSystem> activeComponentSystems = new();

    private void CollectComponentSystems()
    {
        componentSystemData.Clear();
        activeComponentSystems.Clear();

        foreach (var entity in _entities.Values())
        {
            foreach (var data in CollectComponentSystem(entity))
            {
                activeComponentSystems.Add(data.system);
                componentSystemData.Add(data);
            }
        }
    }

    private IEnumerable<(IComponentSystem system, IEntity entity, int start, int end)> CollectComponentSystem(
        IEntity entity)
    {
        for (int i = 0; i < entity.ComponentTypes.Length; i++)
        {
            if (_componentSystems.TryGetValue(entity.ComponentTypes.Span[i], out var componentSystems))
            {
                foreach (var system in componentSystems)
                {
                    var data = FindComponentSystem(entity, system, i);
                    if (data.HasValue)
                    {
                        yield return data.Value;
                    }
                }
            }
        }
    }

    private (IComponentSystem system, IEntity entity, int start, int end)? FindComponentSystem(
        IEntity entity,
        IComponentSystem system,
        int entityStartPointer)
    {
        int systemPointer = 1;
        var entityPointer = entityStartPointer + 1;
        for (; systemPointer < system.CompTypes.Length && entityPointer < entity.ComponentTypes.Length; entityPointer++)
        {
            // cancel if one type does not match, because both types are ordered we know that the rest will not match
            if (entity.ComponentTypes.Span[entityPointer] == system.CompTypes.Span[systemPointer])
            {
                systemPointer++;
            }
        }

        var entityEndPointer = entityPointer;
        var len = systemPointer;
        if (len < system.CompTypes.Length)
        {
            return null;
        }

        return (system, entity, entityStartPointer, entityEndPointer);
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
}