namespace BulletHell.Desktop.Components;

public partial struct BounceMoveComponent : IComponent
{
    public Vector2 Velocity;
    public int BouncesRemaining;
    public float BounceDamping;
    public bool FlipOnBounce;
}
