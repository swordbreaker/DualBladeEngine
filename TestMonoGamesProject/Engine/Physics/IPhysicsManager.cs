using System.Collections.Generic;

namespace TestMonoGamesProject.Engine.Physics;

public interface IPhysicsManager
{
    void AddColliders(IEnumerable<ICollider> colliders);
    bool CheckCollision(ICollider collider);
    void Clear();
}