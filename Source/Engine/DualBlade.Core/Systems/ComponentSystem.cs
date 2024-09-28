using DualBlade.Core.Components;
using DualBlade.Core.Services;

namespace DualBlade.Core.Systems;

public abstract class ComponentSystem<TComponent>(IGameContext gameContext) : BaseSystem(gameContext), IComponentSystem
    where TComponent : IComponent
{
    Type IComponentSystem.CompType { get; } = typeof(TComponent);

    public override void Initialize() { }

    public override void Update(GameTime gameTime) { }
    public override void Draw(GameTime gameTime) { }
    protected virtual void Update(ref TComponent component, GameTime gameTime) { }
    protected virtual void Draw(TComponent component, GameTime gameTime) { }
    protected virtual void OnAdded(ref TComponent component) { }
    protected virtual void OnDestroy(TComponent component) { }

    IComponent IComponentSystem.Update(IComponent component, GameTime gameTime)
    {
        var comp = (TComponent)component;

        this.Update(ref comp, gameTime);
        return comp;
    }

    void IComponentSystem.Draw(IComponent component, GameTime gameTime) =>
        this.Draw((TComponent)component, gameTime);

    IComponent IComponentSystem.OnAdded(IComponent component)
    {
        var c = (TComponent)component;
        OnAdded(ref c);
        return c;
    }

    void IComponentSystem.OnDestroy(IComponent component) =>
        OnDestroy((TComponent)component);

    public override void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
