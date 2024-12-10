using DualBlade._2D.BladePhysics.Services;

namespace FluidBattle;

public class PhysicsSettings : IPhysicsSettings
{
    public IGridSettings GridSettings { get; } = new UniformGirdSettings(CellSize: 20f / 32, Width: 30, Height: 20);

    public Vector2 Gravity { get; } = Vector2.Zero;
}