using DualBlade.Core.Systems;

namespace DualBlade.Core.Worlds;
public partial class World
{
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
}
