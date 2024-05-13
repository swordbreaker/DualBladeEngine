using Microsoft.Xna.Framework;
using TestMonoGamesProject.Engine.Components;
using TestMonoGamesProject.Engine.World;
using TestMonoGamesProject.Engine.Worlds;

namespace TestMonoGamesProject.Engine.Systems
{
    public abstract class ComponentSystem<TComponent> : IComponentSystem where TComponent : IComponent
    {
        public IWorld World { get; init; }

        protected virtual void Initialize(TComponent component, IGameEngine gameEngine) { }

        protected virtual void Update(TComponent component, GameTime gameTime, IGameEngine gameEngine) { }

        protected virtual void Draw(TComponent component, GameTime gameTime, IGameEngine gameEngine) { }

        public void Initialize(IGameEngine gameEngine)
        {
            foreach (var component in World.GetComponents<TComponent>())
            {
                Initialize(component, gameEngine);
            }
        }

        public void Update(GameTime gameTime, IGameEngine gameEngine)
        {
            foreach (var component in World.GetComponents<TComponent>())
            {
                Update(component, gameTime, gameEngine);
            }
        }

        public void Draw(GameTime gameTime, IGameEngine gameEngine)
        {
            foreach (var component in World.GetComponents<TComponent>())
            {
                Draw(component, gameTime, gameEngine);
            }
        }

        protected TransformComponent GetTransformComponent(IComponent component) =>
            World.GetComponent<TransformComponent>(component.Entity)!;
    }
}
