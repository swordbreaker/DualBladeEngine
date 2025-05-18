using NWaves.Signals;
using NWaves.Windows;

namespace BulletHell.Desktop.Models.FeatureExtraction;

public record BandProperies
{
    public required string Name { get; init; }
    public required int Low { get; init; }
    public required int High { get; init; }
}

public record BeatExtractionProperties
{
    public required DiscreteSignal Signal { get; init; }

    public int FrameSize { get; init; } = 1024;
    public int HopSize { get; init; } = 512;

    public WindowType WindowType { get; init; } = WindowType.Hamming;

    public BandProperies[] BandProperties { get; init; } = [
        new BandProperies
        {
            Name = "Kick",
            Low = 60,
            High = 250
        },
        new BandProperies
        {
            Name = "Snare",
            Low = 250,
            High = 2000
        },
        new BandProperies
        {
            Name = "Hi-Hat",
            Low = 2000,
            High = 6000
        },
    ];
}
