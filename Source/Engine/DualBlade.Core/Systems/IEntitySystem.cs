using DualBlade.Core.Entities;

namespace DualBlade.Core.Systems;

public interface IEntitySystem : ISystemWithContext
{
    internal Type EntityType { get; }
    internal IEntity OnAdded(IEntity entity);
    internal void OnDestroy(IEntity entity);

    internal IEntity Update(IEntity entity, GameTime gameTime);
    internal void Draw(IEntity entity, GameTime gameTime);
    void AfterDraw(GameTime gameTime);
}
