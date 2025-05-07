using System;
using System.IO;
using BulletHell.Desktop.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using FmodForFoxes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BulletHell.Desktop.Services;


public class MusicSystem(IGameContext context) : ComponentSystem<MusicComponent>(context)
{
    private readonly FeatureExtraction featureExtraction = new FeatureExtraction();
    private readonly IGameEngine gameEngine = context.GameEngine;

    private Texture2D _pixel;
    private float lastFeatureValue = 0f;

    public override void Initialize()
    {
        var nativeLibrary = new DesktopNativeFmodLibrary();
        FmodManager.Init(nativeLibrary, FmodInitMode.CoreAndStudio, "./");


        _pixel = new(gameEngine.SpriteBatch.GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);
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
        var features = featureExtraction.Extract(component.Signal);
        component.Features = features;

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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="features"></param>
    /// <param name="featureIndex"></param>
    /// <param name="miliseconds"></param>
    /// <param name="hopDuration">In miliseconds</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="ArgumentException"></exception>
    private float GetFeatureValue(float[][] features, int featureIndex, float miliseconds, float hopDuration)
    {
        if (featureIndex < 0 || featureIndex >= features.Length)
            throw new ArgumentOutOfRangeException(nameof(featureIndex));

        if (miliseconds < 0)
            throw new ArgumentOutOfRangeException(nameof(miliseconds));
        if (hopDuration <= 0)
            throw new ArgumentOutOfRangeException(nameof(hopDuration));
        if (features.Length == 0)
            throw new ArgumentException("Feature array is empty", nameof(features));

        // Calculate the time index based on the hop duration
        int timeIndex = (int)(miliseconds / hopDuration);
        if (timeIndex < 0 || timeIndex >= features.Length)
            throw new ArgumentOutOfRangeException(nameof(timeIndex));

        // Ensure the time index is within the bounds of the feature array
        if (timeIndex >= features.Length)
            timeIndex = features.Length - 1;
        if (timeIndex < 0)
            timeIndex = 0;

        return features[timeIndex][featureIndex];
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

        component.Channel.getPosition(out var ms, FMOD.TIMEUNIT.MS);

        lastFeatureValue = GetFeatureValue(
            component.Features,
            0,
            ms,
            250
        );
    }

    private Color CentroidToColor(float centroid)
    {
        // range is eta 1000 to 7000 

        // Normalize the centroid value to a range of 0-1
        float normalizedCentroid = Math.Clamp(centroid/7000, 0f, 1f);

        // Map the normalized value to a color (e.g., grayscale)
        byte colorValue = (byte)(normalizedCentroid * 255);
        return new Color(colorValue, colorValue, colorValue);
    }

    public override void Draw(GameTime gameTime)
    {
        var color = CentroidToColor(lastFeatureValue);

        gameEngine.BeginDraw();
        gameEngine.Draw(_pixel, Vector2.Zero, color, sourceRectangle: new Rectangle(0, 0, 200, 200));

        gameEngine.EndDraw();
    }
}