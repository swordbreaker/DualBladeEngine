using DualBlade.Core.Collections;
using DualBlade.Core.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Systems;

namespace DualBlade.Core.Worlds;
public partial class World
{
    private delegate void UpdateFunc(IComponentSystem system, Span<IComponent> components);
    private readonly GrowableMemory<(IComponentSystem system, IEntity entity, int start, int end)> _componentSystemData = new(100);
    private readonly HashSet<IComponentSystem> _activeComponentSystems = new();

    private void CollectComponentSystems()
    {
        _componentSystemData.Clear();
        _activeComponentSystems.Clear();

        foreach (var entity in _entities.Values())
        {
            foreach (var data in CollectComponentSystem(entity))
            {
                _activeComponentSystems.Add(data.system);
                _componentSystemData.Add(data);
            }
        }
    }

    private IEnumerable<(IComponentSystem system, IEntity entity, int start, int end)> CollectComponentSystem(IEntity entity)
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

    //private unsafe void IterateComponentSystems(IEntity entity, UpdateFunc updateFunc)
    //{
    //    for (int i = 0; i < entity.ComponentTypes.Length; i++)
    //    {
    //        if (_componentSystems.TryGetValue(entity.ComponentTypes.Span[i], out var componentSystems))
    //        {
    //            UpdateComponentSystemsForEntity(entity, updateFunc, componentSystems, i);
    //        }
    //    }
    //}

    //private static unsafe void UpdateComponentSystemsForEntity(IEntity entity, UpdateFunc updateFunc, List<IComponentSystem> componentSystems, int entityStartPointer)
    //{
    //    foreach (var system in componentSystems)
    //    {
    //        ProcessComponentSystem(entity, updateFunc, entityStartPointer, system);
    //    }
    //}

    //private static unsafe void ProcessComponentSystem(IEntity entity, UpdateFunc updateFunc, int entityStartPointer, IComponentSystem system)
    //{
    //    Span<IComponent> components = new IComponent[system.CompTypes.Length];
    //    components[0] = entity.InternalComponents[entityStartPointer];

    //    int systemPointer = 1;
    //    var entityPointer = entityStartPointer + 1;
    //    for (; systemPointer < system.CompTypes.Length && entityPointer < entity.ComponentTypes.Length; entityPointer++)
    //    {
    //        // cancel if one type does not match, because both types are ordered we know that the rest will not match
    //        if (entity.ComponentTypes.Span[entityPointer] == system.CompTypes.Span[systemPointer])
    //        {
    //            components[systemPointer] = entity.InternalComponents[entityPointer];
    //            systemPointer++;
    //        }
    //    }

    //    var entityEndPointer = entityPointer;
    //    var len = systemPointer;
    //    if (len < system.CompTypes.Length)
    //    {
    //        return;
    //    }

    //    // The slice also does not work here because there can be gaps
    //    updateFunc(system, components);
    //}

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
