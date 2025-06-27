using System;
using System.Collections.Generic;
using BulletHell.Desktop.Components;
using BulletHell.Desktop.Models;

public class MusicFeatureConverter(Vector2 GameSize)
{
    private const int HopDuration = 250;
    private float[] lastFeatureValues = [];
    private List<SpectogramFeature> lastSpecPoints = [];
    private bool[] isBeat = [];
    private int[] lastBeatIndexes = [];

    public void Update(MusicComponent musicComponent)
    {
        musicComponent.Channel.getPosition(out var ms, FMOD.TIMEUNIT.MS);

        var timeIndex = CalculateTimeIndex(ms, HopDuration);
        lastFeatureValues = musicComponent.Features[timeIndex];

        var hopDuration = musicComponent.Length / musicComponent.SpectrogramPoints.Count;
        lastSpecPoints = musicComponent.SpectrogramPoints[CalculateTimeIndex(ms, hopDuration)];

        if (lastBeatIndexes.Length == 0)
        {
            lastBeatIndexes = new int[musicComponent.Beats.Length];
            isBeat = new bool[musicComponent.Beats.Length];
        }

        var seconds = ms / 1000f;

        for (int i = 0; i < musicComponent.Beats.Length; i++)
        {
            List<float> band = musicComponent.Beats[i];
            if (band[lastBeatIndexes[i]] < seconds - 0.01f)
            {
                isBeat[i] = true;
                lastBeatIndexes[i]++;
            }
            else
            {
                isBeat[i] = false;
            }
        }
    }

    public bool IsReady => lastFeatureValues.Length > 0;

    public bool IsBeat(int beatIndex)
    {
        return isBeat[beatIndex];
    }

    public float CurrentNormalizedCentroid =>
        NormalizedCentroid(lastFeatureValues[0]);

    public Color CurrentCentriodColor =>
        CentroidToColor(lastFeatureValues[0]);

    public Color CurrentSpreadColor =>
        SpreadToColor(lastFeatureValues[1]);

    public float CurrentNormalizedDecrease =>
        NormalizedDecreate(lastFeatureValues[2]);

    public Color CurrentDecreaseColor =>
        DecreaseToColor(lastFeatureValues[2]);

    public float CurrentRMS =>
        lastFeatureValues[3];

    public Color CurrentRMSColor =>
        RMSColor(lastFeatureValues[3]);

    public Color CurrentZCRolor =>
        ZCRColor(lastFeatureValues[4]);


    public IEnumerable<SpectogramPoint> GetCurrentSpectrogramBullets()
    {
        foreach (var point in lastSpecPoints)
        {
            var x = (point.RelativeValue * GameSize.X) - (GameSize.X / 2);
            yield return new SpectogramPoint
            {
                Point = new Vector2(x, GameSize.Y / 2),
                Frequency = point.Frequency,
                Decibel = point.Decibel,
                RelativeValue = point.RelativeValue
            };
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

    private float NormalizedCentroid(float centroid)
    {
        // range is eta 1000 to 7000
        return Math.Clamp(centroid / 7000, 0f, 1f);
    }

    private Color CentroidToColor(float centroid)
    {
        // range is eta 1000 to 7000 

        // Normalize the centroid value to a range of 0-1
        float normalizedCentroid = NormalizedCentroid(centroid);

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
        float normalizedDecrease = NormalizedDecreate(decrease);

        // Map the normalized value to a color (e.g., grayscale)
        byte colorValue = (byte)(normalizedDecrease * 255);
        return new Color(colorValue, colorValue, colorValue);
    }

    private float NormalizedDecreate(float decrease) =>
        // Decrease is in range of -1 to 1
        Math.Clamp((decrease + 1) / 2, 0f, 1f);

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