using DualBlade._2D.BladePhysics.Models;

namespace DualBlade._2D.BladePhysics.Services;
public class PhysicsManager : IPhysicsManager
{
    private readonly UniformGrid uniformGrid;

    public PhysicsManager(IPhysicsSettings physicsSettings)
    {
        switch (physicsSettings.GridSettings)
        {
            case IUniformGirdSettings uniformGridSettings:
                uniformGrid = new UniformGrid(uniformGridSettings.CellSize, uniformGridSettings.Width, uniformGridSettings.Height);
                break;
        }
    }

    public void Add(ICollider collider, Vector2 pos) =>
        uniformGrid.Insert(collider, pos);

    public void Remove(ICollider collider, Vector2 pos) => uniformGrid.Remove(collider, pos);

    public void Update(ICollider collider, Vector2 oldPos, Vector2 newPos) =>
        uniformGrid.Update(collider, oldPos, newPos);

    public IEnumerable<CollisionInfo> GetCollisions(ICollider collider)
    {
        foreach (var otherCollider in uniformGrid.Query(collider))
        {
            if (collider.HitTest(otherCollider, out var info))
            {
                yield return info;
            }
        }
    }
}
