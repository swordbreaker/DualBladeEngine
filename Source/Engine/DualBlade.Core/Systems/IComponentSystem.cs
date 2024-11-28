using DualBlade.Core.Components;
using DualBlade.Core.Entities;

namespace DualBlade.Core.Systems;

public interface IComponentSystem : ISystemWithContext
{
    Memory<Type> CompTypes { get; }

    void OnAdded(IEntity entity, Span<IComponent> components, out IEntity outEntity,
        out Span<IComponent> outComponents);

    void OnDestroy(IEntity entity, Span<IComponent> component);
    void LateUpdate(GameTime gameTime);
    void FixedUpdate(GameTime gameTime);

    void Update(IEntity entity, Span<IComponent> components, GameTime gameTime, out IEntity outEntity,
        out Span<IComponent> outComponents);

    void FixedUpdate(IEntity entity, Span<IComponent> components, GameTime gameTime, out IEntity outEntity,
        out Span<IComponent> outComponents);

    void Draw(IEntity entity, Span<IComponent> components, GameTime gameTime);
    void LateDraw(GameTime gameTime);
}