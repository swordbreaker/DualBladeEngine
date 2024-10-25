namespace DualBlade._2D.BladePhysics.Services;
public interface IPhysicsSettings
{
    IGridSettings GridSettings { get; }

    Vector2 Gravity { get; }
}

public interface IGridSettings
{
}

public interface IUniformGirdSettings : IGridSettings
{
    int CellSize { get; }
    float Width { get; }
    float Height { get; }
}