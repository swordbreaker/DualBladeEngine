using DualBlade.Core.Components;
using DualBlade.Core.Entities;

namespace DualBlade.Core.Systems;

public interface IComponentSystem : ISystemWithContext
{
    internal Memory<Type> CompTypes { get; }

    internal void OnAdded(IEntity entity, Span<IComponent> components, out IEntity outEntity, out Span<IComponent> outComponents);
    internal void OnDestroy(IEntity entity, Span<IComponent> component);
    internal void Update(IEntity entity, Span<IComponent> components, GameTime gameTime, out IEntity outEntity, out Span<IComponent> outComponents);
    internal void Draw(IEntity entity, Span<IComponent> components, GameTime gameTime);
    void AfterDraw(GameTime gameTime);
}
