using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TestMonoGamesProject.Engine.Components;
using TestMonoGamesProject.Engine.Entities;
using TestMonoGamesProject.Engine.Systems;
using TestMonoGamesProject.Engine.Worlds;

namespace TestMonoGamesProject.Engine.World
{
    public class World(IGameEngine _gameEngine) : IWorld
    {
        private readonly HashSet<ISystem> _systems = [];
        private readonly Dictionary<Type, HashSet<IComponent>> _components = [];
        private readonly Dictionary<Type, HashSet<IEntity>> _entities = [];

        public IEnumerable<IComponent> Components => _components.Values.SelectMany(x => x);
        public IEnumerable<ISystem> Systems => _systems;
        public IEnumerable<IEntity> Entities => _entities.Values.SelectMany(x => x);

        public void Initialize()
        {
            foreach (var system in _systems)
            {
                system.Initialize(_gameEngine);
            }
        }

        public void AddSystems(params ISystem[] systems)
        {
            _systems.UnionWith(systems);
        }

        public void AddEntities(params IEntity[] entities)
        {
            foreach (var entity in entities)
            {
                AddEntity(entity);
            }
        }

        public void AddComponent<TComponent>(IEntity entity, TComponent component) where TComponent : IComponent
        {
            if (!_entities.ContainsKey(entity.GetType()) || !_entities[entity.GetType()].Contains(entity))
            {
                throw new Exception("Entity not found in world");
            }

            if (!_components.ContainsKey(component.GetType()))
            {
                _components[component.GetType()] = [];
            }

            _components[component.GetType()].Add(component);

            if (_entities[entity.GetType()].TryGetValue(entity, out var e))
            {
                e.AddComponent(component);
            }
        }

        public void AddEntity(IEntity entity)
        {
            if (!_entities.ContainsKey(entity.GetType()))
            {
                _entities[entity.GetType()] = new HashSet<IEntity>();
            }

            _entities[entity.GetType()].Add(entity);
            foreach (var component in entity.Components)
            {
                if (!_components.ContainsKey(component.GetType()))
                {
                    _components[component.GetType()] = [];
                }

                _components[component.GetType()].Add(component);
            }
        }

        public void Destroy(IEntity entity)
        {
            foreach (var component in entity.Components)
            {
                Destroy(entity, component);
            }

            _entities[entity.GetType()].Remove(entity);
        }

        public void Destroy(IEntity entity, IComponent component)
        {
            if (!_entities.ContainsKey(entity.GetType()) || !_entities[entity.GetType()].TryGetValue(entity, out var e))
            {
                throw new Exception("Entity not found in world");
            }

            if (!_components.ContainsKey(component.GetType()) || !_components[component.GetType()].TryGetValue(component, out var c))
            {
                throw new Exception("Component not found in world");
            }

            entity.RemoveComponent(component.GetType());
            _components[component.GetType()].Remove(component);
        }

        public void DestroyComponent<TComponent>(IEntity entity) where TComponent : IComponent
        {
            entity.RemoveComponent<TComponent>();
            _components[typeof(TComponent)].RemoveWhere(c => c.Entity == entity);
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
            foreach (var system in _systems)
            {
                system.Update(gameTime, _gameEngine);
            }

            _gameEngine.PhysicsManager.Step(gameTime.ElapsedGameTime);
        }

        public void Draw(GameTime gameTime)
        {
            _gameEngine.BeginDraw();
            foreach (var system in _systems)
            {
                system.Draw(gameTime, _gameEngine);
            }
            _gameEngine.EndDraw();
        }

        public void AddSystem<TSystem>() where TSystem : ISystemWithWorld, new()
        {
            this.AddSystems(this.CreateSystem<TSystem>());
        }
    }
}
