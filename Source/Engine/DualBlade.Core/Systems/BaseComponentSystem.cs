using DualBlade.Core.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;

namespace DualBlade.Core.Systems;

public abstract class BaseComponentSystem(IGameContext gameContext) : BaseSystem(gameContext), IComponentSystem
{
    public abstract Memory<Type> CompTypes { get; }

    public override void Initialize()
    {
    }

    public override void Update(GameTime gameTime)
    {
    }

    public virtual void LateUpdate(GameTime gameTime)
    {
    }

    public override void Draw(GameTime gameTime)
    {
    }

    public virtual void LateDraw(GameTime gameTime)
    {
    }

    public override void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    void IComponentSystem.OnAdded(IEntity entity, Span<IComponent> components, out IEntity outEntity,
        out Span<IComponent> outComponents) =>
        throw new NotImplementedException();

    void IComponentSystem.OnDestroy(IEntity entity, Span<IComponent> component)
    {
    }

    void IComponentSystem.Draw(IEntity entity, Span<IComponent> components, GameTime gameTime)
    {
    }

    void IComponentSystem.Update(IEntity entity, Span<IComponent> components, GameTime gameTime, out IEntity outEntity,
        out Span<IComponent> outComponents) =>
        throw new NotImplementedException();

    void IComponentSystem.FixedUpdate(IEntity entity, Span<IComponent> components, GameTime gameTime,
        out IEntity outEntity,
        out Span<IComponent> outComponents) =>
        throw new NotImplementedException();
}