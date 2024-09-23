using DualBlade.Core.Components;
using DualBlade.Core.Services;

namespace DualBlade.Core.Systems;

public abstract class ComponentSystem<TComponent>(IGameContext gameContext) : BaseSystem(gameContext), IComponentSystem where TComponent : IComponent
{
    protected virtual void Initialize(TComponent component) { }

    protected virtual void OnDestroy(TComponent component) { }

    protected virtual void Update(TComponent component, GameTime gameTime) { }

    protected virtual void Draw(TComponent component, GameTime gameTime) { }

    public override void Initialize()
    {
        foreach (var component in World.GetComponents<TComponent>())
        {
            Initialize(component);
        }

        World.ComponentAdded += Init;
        World.ComponentDestroyed += OnDestroy;
    }

    public override void Update(GameTime gameTime)
    {
        foreach (var component in World.GetComponents<TComponent>())
        {
            Update(component, gameTime);
        }
    }

    public override void Draw(GameTime gameTime)
    {
        foreach (var component in World.GetComponents<TComponent>())
        {
            Draw(component, gameTime);
        }
    }

    public override void Dispose()
    {
        GC.SuppressFinalize(this);
        World.ComponentAdded -= Init;
        World.ComponentDestroyed -= OnDestroy;
    }

    private void Init(IComponent component)
    {
        if (component is TComponent c)
        {
            Initialize(c);
        }
    }

    private void OnDestroy(IComponent component)
    {
        if (component is TComponent c)
        {
            OnDestroy(c);
        }
    }
}
