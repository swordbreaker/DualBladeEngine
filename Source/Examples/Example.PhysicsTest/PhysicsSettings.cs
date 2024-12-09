using DualBlade._2D.BladePhysics.Services;

namespace Example.PhysicsTest;

public class PhysicsSettings : IPhysicsSettings
{
    public IGridSettings GridSettings { get; } = new UniformGirdSettings(CellSize: 9f / 32, Width: 10, Height: 10);

    public Vector2 Gravity { get; } = Vector2.Zero;
}