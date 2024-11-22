using DualBlade._2D.BladePhysics.Services;

namespace FluidBattle;

public class PhysicsSettings : IPhysicsSettings
{
    public IGridSettings GridSettings { get; } = new UniformGirdSettings(CellSize: 0.1f, Width: 10, Height: 10);

    public Vector2 Gravity { get; } = Vector2.Zero;
}