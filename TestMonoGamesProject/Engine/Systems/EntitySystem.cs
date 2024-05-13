using Microsoft.Xna.Framework;
using TestMonoGamesProject.Engine.Entities;
using TestMonoGamesProject.Engine.World;
using TestMonoGamesProject.Engine.Worlds;

namespace TestMonoGamesProject.Engine.Systems
{
    public class EntitySystem<TEntity> : IEntitySystem where TEntity : IEntity
    {
        public IWorld World { get; init; }

        protected virtual void Initialize(TEntity entity, IGameEngine gameEngine) { }
        protected virtual void Update(TEntity entity, GameTime gameTime, IGameEngine gameEngine) { }
        protected virtual void Draw(TEntity entity, GameTime gameTime, IGameEngine gameEngine) { }

        public void Initialize(IGameEngine gameEngine)
        {
            foreach (var entity in World.GetEntities<TEntity>())
            {
                Initialize(entity, gameEngine);
            }
        }

        public void Update(GameTime gameTime, IGameEngine gameEngine)
        {
            foreach (var entity in World.GetEntities<TEntity>())
            {
                Update(entity, gameTime, gameEngine);
            }
        }

        public void Draw(GameTime gameTime, IGameEngine gameEngine)
        {
            foreach (var entity in World.GetEntities<TEntity>())
            {
                Draw(entity, gameTime, gameEngine);
            }
        }
    }
}
