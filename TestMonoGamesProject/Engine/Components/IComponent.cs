using TestMonoGamesProject.Engine.Entities;

namespace TestMonoGamesProject.Engine.Components
{
    public interface IComponent
    {
        IEntity Entity { init; get; }
    }
}
