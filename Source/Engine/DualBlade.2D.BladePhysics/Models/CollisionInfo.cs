namespace DualBlade._2D.BladePhysics.Models;

/// <summary>
/// 
/// </summary>
/// <param name="Collider"></param>
/// <param name="OtherCollider"></param>
/// <param name="Normal"> The normal of the collision, pointing from the other collider to the collider.
/// The collider has the following properties:
/// - Direction: The collision normal points from one object to the other, from the object being collided with towards the colliding object
/// - Perpendicularity: For simple shapes like spheres, the collision normal is perpendicular to the surfaces at the point of contact
/// - Separation Vector: It represents the direction in which the objects need to be separated to resolve the collision
/// </param>
/// <param name="PenetrationDepth"></param>
/// <param name="ContactPoint"></param>
public record struct CollisionInfo(
    ICollider Collider,
    ICollider OtherCollider,
    Vector2 Normal,
    float PenetrationDepth,
Vector2 ContactPoint);