using DualBlade.Core.Collections;
using DualBlade.Core.Entities;
using DualBlade.Core.Systems;

namespace DualBlade.Core.Worlds
{
    public partial class World
    {
        private readonly GrowableMemory<(IEntitySystem, IEntity)> _entitySystemsData = new(100);
        private readonly HashSet<IEntitySystem> _activeEntitySystems = new();

        private void CollectEntitySystems()
        {
            _entitySystemsData.Clear();
            _activeEntitySystems.Clear();

            foreach (var entity in _entities.Values())
            {
                if (_entitySystems.TryGetValue(entity.GetType(), out var systems))
                {
                    foreach (var system in systems)
                    {
                        _activeEntitySystems.Add(system);
                        _entitySystemsData.Add((system, entity));
                    }
                }
            }
        }
    }
}
