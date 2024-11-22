using DualBlade._2D.BladePhysics.Components;
using DualBlade._2D.BladePhysics.Models;

namespace DualBlade._2D.BladePhysics.Services;

public interface IPhysicsManager
{
    void Add(ICollider collider);
    IEnumerable<CollisionInfo> CalculateCollisions(ICollider collider);
    IEnumerable<CollisionInfo> GetCollisions(RigidBody rigidBody);
    IEnumerable<CollisionInfo> GetNewCollisions(RigidBody rigidBody);
    IEnumerable<CollisionInfo> GetRemovedCollisions(RigidBody rigidBody);
    void Remove(ICollider collider);
    void SetCollisions(RigidBody rigidBody, IEnumerable<CollisionInfo> collisions);
    void Update(ICollider collider, Vector2 oldPos, Vector2 newPos);
}