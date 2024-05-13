using TestMonoGamesProject.Engine.Worlds;

namespace TestMonoGamesProject.Engine.World
{
    public class WorldFactory
    {
        public IWorld Create(IGameEngine engine) =>
            new World(engine);
    }
}
