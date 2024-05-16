
using Microsoft.Xna.Framework;

namespace TestMonoGamesProject.Engine.Physics;

public class PhysicsManager : nkast.Aether.Physics2D.Dynamics.World
{
    public PhysicsManager(Vector2 gravity) : base(gravity)
    {
    }

    public PhysicsManager() : base() { }

    //private List<ICollider> colliders = new();


    //public void Update(GameTime gameTime)
    //{
    //    _physicsWorld.Step(gameTime.ElapsedGameTime);
    //}

    //public void AddColliders(IEnumerable<ICollider> colliders)
    //{
    //    this.colliders.AddRange(colliders);
    //}

    //public bool CheckCollision(ICollider collider)
    //{
    //    foreach (var c in colliders.Where(c => !c.Equals(collider)))
    //    {
    //        if (c.IntersectsWith(collider))
    //        {
    //            return true;
    //        }
    //    }

    //    return false;
    //}

    //public Vector2 GetCollisionNormal(ICollider collider)
    //{
    //    foreach (var c in colliders.Where(c => !c.Equals(collider)))
    //    {
    //        if (c.IntersectsWith(collider))
    //        {
    //            return new Vector2(collider.BoundingBox.Location.X - c.BoundingBox.Location.X, collider.BoundingBox.Location.Y - c.BoundingBox.Location.Y);
    //        }
    //    }

    //    return Vector2.Zero;
    //}

    //public void Clear() => colliders.Clear();
}