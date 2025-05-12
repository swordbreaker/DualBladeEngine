using System;
using System.Collections.Generic;
using BulletHell.Desktop.Components;
using BulletHell.Desktop.Helpers;

public class MusicFeatureConverter(Vector2 GameSize)
{
    private const int HopDuration = 250;
    private float[] lastFeatureValues = [];
    private List<float> lastSpecPoints = [];

    public void Update(MusicComponent musicComponent)
    {
        musicComponent.Channel.getPosition(out var ms, FMOD.TIMEUNIT.MS);

        var timeIndex = CalculateTimeIndex(ms, HopDuration);
        lastFeatureValues = musicComponent.Features[timeIndex];

        var hopDuration = musicComponent.Length / musicComponent.SpectrogramPoints.Count;
        lastSpecPoints = musicComponent.SpectrogramPoints[CalculateTimeIndex(ms, hopDuration)];
    }

    public Color CurrentCentriodColor =>
        CentroidToColor(lastFeatureValues[0]);

    public Color CurrentSpreadColor =>
        SpreadToColor(lastFeatureValues[1]);

    public Color CurrentDecreaseColor =>
        DecreaseToColor(lastFeatureValues[2]);

    public Color CurrentRMSColor =>
        RMSColor(lastFeatureValues[3]);

    public Color CurrentZCRolor =>
        ZCRColor(lastFeatureValues[4]);


    public IEnumerable<Vector2> GetCurrentSpectrogramBullets()
    {
        foreach (var point in lastSpecPoints)
        {
            var x = (point * GameSize.X) - (GameSize.X / 2);
            yield return new Vector2(x, GameSize.Y / 2);
        }
    }

    private int CalculateTimeIndex(float miliseconds, float hopDuration)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(miliseconds);
        ArgumentOutOfRangeException.ThrowIfNegative(hopDuration);

        // Calculate the time index based on the hop duration
        int timeIndex = (int)(miliseconds / hopDuration);
        return timeIndex;
    }

    private Color CentroidToColor(float centroid)
    {
        // range is eta 1000 to 7000 

        // Normalize the centroid value to a range of 0-1
        float normalizedCentroid = Math.Clamp(centroid / 7000, 0f, 1f);

        // Map the normalized value to a color (e.g., grayscale)
        byte colorValue = (byte)(normalizedCentroid * 255);
        return new Color(colorValue, colorValue, colorValue);
    }

    private Color SpreadToColor(float spread)
    {
        // Spead is in range of 2000 to 6000

        // Normalize the spread value to a range of 0-1
        float normalizedSpread = Math.Clamp(spread / 6000, 0f, 1f);

        // Map the normalized value to a color (e.g., grayscale)
        byte colorValue = (byte)(normalizedSpread * 255);
        return new Color(colorValue, colorValue, colorValue);
    }

    private Color DecreaseToColor(float decrease)
    {
        // Decrease is in range of -1 to 1

        // Normalize the decrease value to a range of 0-1
        float normalizedDecrease = Math.Clamp((decrease + 1) / 2, 0f, 1f);

        // Map the normalized value to a color (e.g., grayscale)
        byte colorValue = (byte)(normalizedDecrease * 255);
        return new Color(colorValue, colorValue, colorValue);
    }

    private Color RMSColor(float rms) => NormalizedToColor(rms);

    private Color ZCRColor(float zcr)
    {
        // ZCR is in range of 0 to 1

        // Normalize the ZCR value to a range of 0-1
        float normalizedZCR = Math.Clamp(zcr, 0f, 0.3f);

        // Map the normalized value to a color (e.g., grayscale)
        byte colorValue = (byte)(normalizedZCR * 255);
        return new Color(colorValue, colorValue, colorValue);
    }

    private Color NormalizedToColor(float value)
    {
        // Factor is in range of 0 to 1

        // Normalize the factor value to a range of 0-1
        float normalizedFactor = Math.Clamp(value, 0f, 1f);

        // Map the normalized value to a color (e.g., grayscale)
        byte colorValue = (byte)(normalizedFactor * 255);
        return new Color(colorValue, colorValue, colorValue);
    }
}