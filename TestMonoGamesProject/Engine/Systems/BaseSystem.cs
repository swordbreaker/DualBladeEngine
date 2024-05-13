using Microsoft.Xna.Framework;
using TestMonoGamesProject.Engine.World;
using TestMonoGamesProject.Engine.Worlds;

namespace TestMonoGamesProject.Engine.Systems;
public class BaseSystem : ISystemWithWorld
{
    public IWorld World { get; init; }

    public virtual void Draw(GameTime gameTime, IGameEngine gameEngine) { }
    public virtual void Initialize(IGameEngine gameEngine) { }
    public virtual void Update(GameTime gameTime, IGameEngine gameEngine) { }
}
