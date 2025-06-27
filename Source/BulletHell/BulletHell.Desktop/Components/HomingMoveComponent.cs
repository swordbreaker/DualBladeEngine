namespace BulletHell.Desktop.Components;

public partial struct HomingMoveComponent : IComponent
{
    public Vector2 Velocity;
    public Vector2 TargetPosition;
    public float TurnSpeed;
    public float MaxSpeed;
    public float AcquisitionRange;
    public bool HasTarget;
    public int TargetEntityId;
}
