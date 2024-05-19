using MonoGameEngine.Engine.Components;
using MonoGameEngine.Engine.Worlds;

namespace MonoGameEngine.Engine.Systems;

public abstract class ComponentSystem<TComponent> : BaseSystem, IComponentSystem where TComponent : IComponent
{
    protected virtual void Initialize(TComponent component, IGameEngine gameEngine) { }

    protected virtual void Update(TComponent component, GameTime gameTime, IGameEngine gameEngine) { }

    protected virtual void Draw(TComponent component, GameTime gameTime, IGameEngine gameEngine) { }

    public override void Initialize(IGameEngine gameEngine)
    {
        foreach (var component in World.GetComponents<TComponent>())
        {
            Initialize(component, gameEngine);
        }
    }

    public override void Update(GameTime gameTime, IGameEngine gameEngine)
    {
        foreach (var component in World.GetComponents<TComponent>())
        {
            Update(component, gameTime, gameEngine);
        }
    }

    public override void Draw(GameTime gameTime, IGameEngine gameEngine)
    {
        foreach (var component in World.GetComponents<TComponent>())
        {
            Draw(component, gameTime, gameEngine);
        }
    }

    protected TransformComponent GetTransformComponent(IComponent component) =>
        World.GetComponent<TransformComponent>(component.Entity)!;
}
