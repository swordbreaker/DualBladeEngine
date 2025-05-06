using DualBlade.Core.Components;
using NWaves.Signals;

namespace BulletHell.Desktop.Components;

public partial struct MusicComponent : IComponent
{
    public string FilePath;
    public DiscreteSignal Signal;
    public int SamplingRate;

    public FMOD.Channel Channel;

    public float[][] Features;
}