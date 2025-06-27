namespace BulletHell.Desktop.Components;

public partial struct LaserComponent : IComponent
{
    public Vector2 Direction;
    public float Length;
    public float Width;
    public float ChargeTime;
    public float Duration;
    public float ElapsedTime;
    public bool IsCharging;
    public bool IsActive;
    public Color ChargeColor;
    public Color ActiveColor;
}
