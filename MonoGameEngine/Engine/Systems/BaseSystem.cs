using MonoGameEngine.Engine.Worlds;

namespace MonoGameEngine.Engine.Systems;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class BaseSystem : ISystemWithWorld
{
    public IWorld World { get; init; }

    public virtual void Draw(GameTime gameTime, IGameEngine gameEngine) { }
    public virtual void Initialize(IGameEngine gameEngine) { }
    public virtual void Update(GameTime gameTime, IGameEngine gameEngine) { }
}
