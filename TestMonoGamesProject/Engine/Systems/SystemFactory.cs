using TestMonoGamesProject.Engine.World;

namespace TestMonoGamesProject.Engine.Systems
{
    public static class SystemFactory
    {
        public static TSystem CreateSystem<TSystem>(this IWorld world) where TSystem : ISystemWithWorld, new() =>
            new() { World = world };
    }
}
