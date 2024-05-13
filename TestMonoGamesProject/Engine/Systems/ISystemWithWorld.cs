using TestMonoGamesProject.Engine.World;

namespace TestMonoGamesProject.Engine.Systems
{
    public interface ISystemWithWorld : ISystem
    {
        IWorld World { get; init; }
    }
}
