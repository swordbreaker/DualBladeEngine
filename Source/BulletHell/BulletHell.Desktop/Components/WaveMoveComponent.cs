namespace BulletHell.Desktop.Components;

public partial struct WaveMoveComponent : IComponent
{
    public Vector2 Direction;
    public float Speed;
    public float Amplitude;
    public float Frequency;
    public float Phase;
    public Vector2 PerpendicularDirection;
}
