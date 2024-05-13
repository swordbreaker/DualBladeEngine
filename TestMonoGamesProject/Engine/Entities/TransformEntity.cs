using TestMonoGamesProject.Engine.Components;

namespace TestMonoGamesProject.Engine.Entities;

public class TransformEntity : Entity
{
    public TransformComponent Transform { get; init; }

    public TransformEntity()
    {
        Transform = AddComponent<TransformComponent>();
    }
}
