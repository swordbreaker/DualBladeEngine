﻿using Microsoft.Xna.Framework;
using System.Collections.Generic;
using TestMonoGamesProject.Engine.Components;
using TestMonoGamesProject.Engine.Entities;
using TestMonoGamesProject.Engine.Systems;

namespace TestMonoGamesProject.Engine.World
{
    public interface IWorld
    {
        IEnumerable<IComponent> Components { get; }
        IEnumerable<ISystem> Systems { get; }
        IEnumerable<IEntity> Entities { get; }

        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);

        void Destroy(IEntity entity);
        void Destroy(IEntity entity, IComponent component);

        void AddComponent<TComponent>(IEntity entity, TComponent component) where TComponent : IComponent;
        void AddEntity(IEntity entity);
        IEnumerable<TComponent> GetComponents<TComponent>() where TComponent : IComponent;
        TComponent? GetComponent<TComponent>(IEntity entity) where TComponent : IComponent;

        IEnumerable<TEntity> GetEntities<TEntity>() where TEntity : IEntity;

        void AddSystems(params ISystem[] systems);
        void AddSystem<TSystem>() where TSystem : ISystemWithWorld, new();
        void DestroyComponent<TComponent>(IEntity entity) where TComponent : IComponent;
    }
}
