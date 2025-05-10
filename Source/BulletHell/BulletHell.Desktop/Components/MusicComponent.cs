using System.Collections.Generic;
using DualBlade.Core.Components;
using NWaves.Signals;

namespace BulletHell.Desktop.Components;

public partial struct MusicComponent : IComponent
{
    public string FilePath;
    public DiscreteSignal Signal;
    public int SamplingRate;

    /// <summary>
    /// The length of the sound in miliseconds.
    /// </summary>
    public float Length;

    public FMOD.Channel Channel;

    public float[][] Features;

    public List<List<float>> SpectrogramPoints;
}