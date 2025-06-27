namespace BulletHell.Desktop.Components;

public partial struct SpiralMoveComponent : IComponent
{
    public Vector2 InitialVelocity;
    public float AngularVelocity;
    public float RadiusGrowth;
    public float CurrentAngle;
    public Vector2 CenterPoint;
}
