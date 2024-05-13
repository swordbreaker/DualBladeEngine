using TestMonoGamesProject.Engine.Components;
using TestMonoGamesProject.Engine.Entities;
using TestMonoGamesProject.Engine.Physics;

namespace TestMonoGamesProject.Components;
public class ColliderComponent : IComponent
{
    public ICollider Collider;

    public IEntity Entity { get; init; }
}
