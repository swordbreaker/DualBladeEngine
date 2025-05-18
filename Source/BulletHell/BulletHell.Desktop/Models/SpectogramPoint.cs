namespace BulletHell.Desktop.Models;

public record SpectogramFeature
{
    public required float Frequency { get; init; }
    public required float Decibel { get; init; }
    public float RelativeValue { get; init; } = 0;
}

public record SpectogramPoint : SpectogramFeature
{
    public required Vector2 Point { get; init; }
}