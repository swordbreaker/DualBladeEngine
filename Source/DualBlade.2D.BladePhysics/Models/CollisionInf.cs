namespace DualBlade._2D.BladePhysics.Models;
public record struct CollisionInfo(ICollider Collider, ICollider OtherCollider, Vector2 Normal, float PenetrationDepth, Vector2 ContactPoint);
