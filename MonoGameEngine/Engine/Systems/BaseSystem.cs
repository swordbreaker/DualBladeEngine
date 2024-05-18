using Microsoft.Xna.Framework;
using MonoGameEngine.Engine.Worlds;

namespace MonoGameEngine.Engine.Systems;
public class BaseSystem : ISystemWithWorld
{
    public IWorld World { get; init; }

    public virtual void Draw(GameTime gameTime, IGameEngine gameEngine) { }
    public virtual void Initialize(IGameEngine gameEngine) { }
    public virtual void Update(GameTime gameTime, IGameEngine gameEngine) { }
}
