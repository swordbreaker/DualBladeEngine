namespace DualBlade._2D.BladePhysics.Services;

public record PhysicsSettings(IGridSettings GridSettings, Vector2 Gravity) : IPhysicsSettings
{
    public static PhysicsSettings Default => new(new UniformGirdSettings(1, 20, 20), new Vector2(0, -9.81f));
}

public record UniformGirdSettings(int CellSize, float Width, float Height) : IUniformGirdSettings
{
}
