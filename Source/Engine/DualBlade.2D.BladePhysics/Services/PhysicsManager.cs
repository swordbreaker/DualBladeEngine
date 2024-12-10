using DualBlade._2D.BladePhysics.Components;
using DualBlade._2D.BladePhysics.Models;

namespace DualBlade._2D.BladePhysics.Services;

public class PhysicsManager : IPhysicsManager
{
    internal readonly UniformGrid? uniformGrid;

    private readonly Dictionary<int, List<CollisionInfo>> rigidBodyToCollisions = [];
    private readonly Dictionary<int, List<CollisionInfo>> addedCollisions = [];
    private readonly Dictionary<int, List<CollisionInfo>> removedCollisions = [];

    public PhysicsManager(IPhysicsSettings physicsSettings)
    {
        uniformGrid = physicsSettings.GridSettings switch
        {
            IUniformGirdSettings uniformGridSettings => new UniformGrid(uniformGridSettings.CellSize,
                uniformGridSettings.Width, uniformGridSettings.Height),
            _ => null
        };
    }

    public void Add(ICollider collider) =>
        uniformGrid.Insert(collider);

    public void Remove(ICollider collider) => uniformGrid.Remove(collider);

    public void Update(ICollider collider, Vector2 oldPos, Vector2 newPos) =>
        uniformGrid.Update(collider, oldPos, newPos);

    public IEnumerable<CollisionInfo> CalculateCollisions(ICollider collider)
    {
        foreach (var otherCollider in uniformGrid.Query(collider))
        {
            if (collider.HitTest(otherCollider, out var info))
            {
                yield return info;
            }
        }
    }

    public void SetCollisions(RigidBody rigidBody, IEnumerable<CollisionInfo> collisions)
    {
        if (rigidBody.CollectCollisionEvents && rigidBodyToCollisions.TryGetValue(rigidBody.Id, out var oldCollisionInfo))
        {
            var oldCollisions = oldCollisionInfo.Select(x => x.Collider).ToHashSet();
            var newCollisions = collisions.Select(x => x.Collider).ToHashSet();

            var added = newCollisions.Except(oldCollisions);
            var removed = oldCollisions.Except(newCollisions);

            addedCollisions[rigidBody.Id] = collisions.Where(x => added.Contains(x.Collider)).ToList();
            removedCollisions[rigidBody.Id] = collisions.Where(x => removed.Contains(x.Collider)).ToList();
        }

        rigidBodyToCollisions[rigidBody.Id] = collisions.ToList();
    }

    public IEnumerable<CollisionInfo> GetCollisions(RigidBody rigidBody)
    {
        if (rigidBodyToCollisions.TryGetValue(rigidBody.Id, out var collisions))
        {
            return collisions;
        }

        return Array.Empty<CollisionInfo>();
    }

    public IEnumerable<CollisionInfo> GetNewCollisions(RigidBody rigidBody)
    {
        if (!rigidBody.CollectCollisionEvents)
        {
            throw new InvalidOperationException(
                $"RigidBody does not collect collision events, set {nameof(RigidBody)}.{nameof(RigidBody.CollectCollisionEvents)} to true");
        }

        return addedCollisions.TryGetValue(rigidBody.Id, out var collisions) ? collisions : [];
    }

    public IEnumerable<CollisionInfo> GetRemovedCollisions(RigidBody rigidBody)
    {
        if (!rigidBody.CollectCollisionEvents)
        {
            throw new InvalidOperationException(
                $"RigidBody does not collect collision events, set {nameof(RigidBody)}.{nameof(RigidBody.CollectCollisionEvents)} to true");
        }

        return removedCollisions.TryGetValue(rigidBody.Id, out var collisions) ? collisions : [];
    }
}