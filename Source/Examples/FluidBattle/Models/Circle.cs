namespace FluidBattle.Models;
public struct Circle
{
    public float Radius { get; set; }
    public Vector2 Position { get; set; }

    public readonly bool InCircle(Vector2 point) =>
        Vector2.Distance(Position, point) < Radius;
}
