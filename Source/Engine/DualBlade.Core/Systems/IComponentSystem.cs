using DualBlade.Core.Components;

namespace DualBlade.Core.Systems;

public interface IComponentSystem : ISystemWithContext
{
    internal Type CompType { get; }

    internal IComponent OnAdded(IComponent component);
    internal void OnDestroy(IComponent component);

    internal IComponent Update(IComponent component, GameTime gameTime);
    internal void Draw(IComponent component, GameTime gameTime);
}
