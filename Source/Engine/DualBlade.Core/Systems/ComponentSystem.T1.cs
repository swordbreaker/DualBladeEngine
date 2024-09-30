using DualBlade.Core.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;

namespace DualBlade.Core.Systems;

public abstract class ComponentSystem<TComponent>(IGameContext gameContext) : BaseSystem(gameContext), IComponentSystem
    where TComponent : IComponent
{
    Memory<Type> IComponentSystem.CompTypes { get; } = new Type[] { typeof(TComponent) };

    public override void Initialize() { }

    public override void Update(GameTime gameTime) { }
    public override void Draw(GameTime gameTime) { }
    public virtual void AfterDraw(GameTime gameTime) { }
    protected virtual void Update(ref TComponent component, ref IEntity entity, GameTime gameTime) { }
    protected virtual void Draw(TComponent component, IEntity entity, GameTime gameTime) { }
    protected virtual void OnAdded(ref IEntity entity, ref TComponent component) { }
    protected virtual void OnDestroy(TComponent component, IEntity entity) { }

    public override void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    void IComponentSystem.OnAdded(IEntity entity, Span<IComponent> components, out IEntity outEntity, out Span<IComponent> outComponents)
    {
        var c = (TComponent)components[0];
        OnAdded(ref entity, ref c);

        outComponents = new IComponent[] { c };
        outEntity = entity;
    }

    void IComponentSystem.OnDestroy(IEntity entity, Span<IComponent> component) =>
            OnDestroy((TComponent)component[0], entity);

    void IComponentSystem.Draw(IEntity entity, Span<IComponent> components, GameTime gameTime) =>
        this.Draw((TComponent)components[0], entity, gameTime);

    void IComponentSystem.Update(IEntity entity, Span<IComponent> components, GameTime gameTime, out IEntity outEntity, out Span<IComponent> outComponents)
    {
        var c = (TComponent)components[0];
        Update(ref c, ref entity, gameTime);

        outComponents = new IComponent[] { c };
        outEntity = entity;
    }
}
