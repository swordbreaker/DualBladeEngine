using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using BulletHell.Desktop.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using FmodForFoxes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace BulletHell.Desktop.Services;


public class MusicSystem(IGameContext context) : ComponentSystem<MusicComponent>(context)
{
    public override void Initialize()
    {
        var nativeLibrary = new DesktopNativeFmodLibrary();
        FmodManager.Init(nativeLibrary, FmodInitMode.CoreAndStudio, "./");


        // var result = FMOD.Debug.Initialize(
        //     FMOD.DEBUG_FLAGS.LOG | FMOD.DEBUG_FLAGS.TYPE_TRACE,
        //     FMOD.DEBUG_MODE.FILE,
        //     null,
        //     "fmod.log"
        // );

        // if (result != FMOD.RESULT.OK)
        // {
        //     throw new Exception($"FMOD error: {result}");
        // }
        // MediaPlayer.Play()
    }

    protected override void OnAdded(ref IEntity entity, ref MusicComponent component)
    {
        var absolutePath = Path.GetFullPath(component.FilePath);

        var result = CoreSystem.Native.createSound(
            absolutePath,
            FMOD.MODE.CREATESTREAM | FMOD.MODE.ACCURATETIME,
            out var sound
        );

        if (result != FMOD.RESULT.OK)
        {
            throw new Exception($"FMOD error: {result}");
        }

        CoreSystem.Native.getMasterChannelGroup(out var masterGroup);
        CoreSystem.Native.playSound(sound, masterGroup, false, out var channel);

        component.Channel = channel;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        FmodManager.Update();
    }

    public override void Dispose()
    {
        base.Dispose();
        FmodManager.Unload();
    }

    protected override void Update(ref MusicComponent component, ref IEntity entity, GameTime gameTime)
    {
        base.Update(ref component, ref entity, gameTime);

        component.Channel.getPosition(out var position, FMOD.TIMEUNIT.MS);
        float seconds = position / 1000f;

        Console.WriteLine($"Current position: {seconds} seconds");
    }
}