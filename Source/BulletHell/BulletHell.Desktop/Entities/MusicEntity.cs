using BulletHell.Desktop.Components;
using BulletHell.Desktop.Services;
using DualBlade.Core.Entities;

namespace BulletHell.Desktop.Entities;

[RequiredComponent<MusicComponent>]
public partial struct MusicEntity : IEntity
{
    public MusicEntity(string path)
    {
        var audioService = new AudioService();

        var signal = audioService.LoadAudioFile(path);

        var component = new MusicComponent
        {
            FilePath = path,
            Signal = signal,
            SamplingRate = signal.SamplingRate
        };

        AddComponent(component);
    }
}