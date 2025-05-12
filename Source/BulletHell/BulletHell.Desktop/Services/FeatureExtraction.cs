using NWaves.FeatureExtractors.Base;
using NWaves.FeatureExtractors.Multi;
using NWaves.FeatureExtractors.Options;
using NWaves.Signals;
using NWaves.Transforms;
using NWaves.Windows;
using ScottPlot;
using System.IO;
using System.Collections.Generic;
using System;
using System.Linq;

namespace BulletHell.Desktop.Services;

/*
Features

-Centroid
Indicates perceived brightness of a sound (e.g., a violin has a higher centroid than a bass drum)

-Spread
Measures how concentrated or dispersed the spectrum is

Flatness
Used in audio classification (e.g., distinguishing noise from music)

Rolloff
Helps identify high-frequency content (e.g., differentiating voiced speech from unvoiced fricatives)

Crest
Indicates dynamic range (higher values = more transient peaks)

Entropy
Noise detection (higher entropy ≈ more noise-like)

-Decreaase
Useful in timbre analysis (e.g., distinguishing instruments)

Energy
Detecting silence/activity in audio.

-Root Mean Square (RMS)
Loudness estimation.

-Zero Crossing Rate (ZCR)
Discriminating noise (high ZCR) from pitched sounds (low ZCR).

*/


/// <summary>
/// Feature extraction class for audio signals.
/// 
///  
/// </summary>
public class FeatureExtraction
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="signal"></param>
    /// <returns>A vector </returns>
    public float[][] Extract(DiscreteSignal signal)
    {
        var opts = new MultiFeatureOptions
        {
            SamplingRate = signal.SamplingRate,
            HopDuration = 0.25f,
        };

        opts.FeatureList = "centroid, spread, decrease";
        var spectralExtractor = new SpectralFeaturesExtractor(opts);

        opts.FeatureList = "rms, zcr";
        var tdExtractor = new TimeDomainFeaturesExtractor(opts);

        var vectors = FeaturePostProcessing.Join(
            spectralExtractor.ParallelComputeFrom(signal),
            tdExtractor.ParallelComputeFrom(signal)
        );

        return vectors;
    }

    /// <summary>
    /// Generates points representing prominent frequencies from a spectrogram.
    /// </summary>
    /// <param name="signal">The audio signal to analyze</param>
    /// <param name="dbThreshold">The decibel threshold above which frequencies are considered prominent (default: 20)</param>
    /// <returns>A collection where each entry represents a timestep containing normalized frequency points (0-1 scale)</returns>
    public List<List<float>> GenerateSpecPoint(DiscreteSignal signal, float dbThreshold = 20)
    {
        var spec = GenerateSpectogram(signal);
        var result = new List<List<(float freq, float db)>>();

        for (int i = 0; i < spec.Count; i++)
        {
            var points = new List<(float freq, float db)>();
            for (int j = 0; j < spec[i].Length; j++)
            {
                var db = MathF.Log10(spec[i][j] + 0.0001f) * 10;

                if (db > dbThreshold)
                {
                    // Normalize the frequency bin to a value between 0 and 1
                    float normalizedFrequency = MathF.Log10(j + 1);
                    points.Add((normalizedFrequency, db));
                }
            }

            // Add this timestep's points to the result
            result.Add(points);
        }

        var maxFreq = result.SelectMany(x1 => x1.Select(x2 => x2.freq)).Max();
        var minFreq = result.SelectMany(x1 => x1.Select(x2 => x2.freq)).Min();
        var maxDb = result.SelectMany(x1 => x1.Select(x2 => x2.db)).Max();
        var minDb = result.SelectMany(x1 => x1.Select(x2 => x2.db)).Min();

        // Normalize the points to a range of 0 to 1
        return [.. result.Select(p =>
        {
            var normalized = p.Select(x =>
            {
                var freqNormalized = (x.freq - minFreq) / (maxFreq - minFreq);
                var dbNormalized = (x.db - minDb) / (maxDb - minDb);
                var dbLog = MathF.Log10(dbNormalized + 1);
                return freqNormalized + dbLog;
            }).ToList();
            return normalized;
        })];
    }

    public List<float[]> GenerateSpectogram(
        DiscreteSignal signal,
        int windowSize = 2048,
        int? hopSize = null,
        WindowType windowType = WindowType.Hann
    )
    {
        // Set default hop size if not provided
        int actualHopSize = hopSize ?? windowSize / 4;

        var hopCount = (signal.Length - windowSize) / actualHopSize + 1;

        // Create the Stft transformer
        var stft = new Stft(windowSize, actualHopSize, windowType);

        // x is the time domain signal
        // y is the frequency domain signal
        return stft.Spectrogram(signal);
    }

    /// <summary>
    /// Generates a spectrogram from an audio signal and saves it as an image
    /// </summary>
    /// <param name="signal">The audio signal to analyze</param>
    /// <param name="outputPath">Path where the spectrogram image will be saved</param>
    /// <param name="windowSize">Size of the FFT window (default 1024)</param>
    /// <param name="hopSize">Hop size between consecutive frames (default windowSize/4)</param>
    /// <param name="windowType">Window function to use (default Hann)</param>
    /// <returns>Path to the generated spectrogram image</returns>
    public string PlotSpectrogram(
        DiscreteSignal signal,
        string outputPath,
        int windowSize = 2048,
        int? hopSize = null,
        WindowType windowType = WindowType.Hann)
    {
        // x is the time domain signal
        // y is the frequency domain signal
        var spectrogram = GenerateSpectogram(signal, windowSize, hopSize, windowType);

        Console.WriteLine($"Spectrogram size: {spectrogram.Count} x {spectrogram[0].Length}");

        // var intensities = new double[spectrogram.Count, spectrogram[0].Length];
        var intensities = new double[spectrogram[0].Length, spectrogram.Count];
        for (int i = 0; i < spectrogram.Count; i++)
        {
            for (int j = 0; j < spectrogram[i].Length; j++)
            {
                var db = Math.Log10(spectrogram[i][j] + 0.0001) * 10;
                intensities[j, i] = db;
            }
        }

        // Create a new plot with ScottPlot 5.x
        var plt = new Plot();

        // Add heatmap - note the parameters might differ in your specific version
        var heatmap = plt.Add.Heatmap(intensities);

        // Set colormap - use default since we're unsure of the exact API for your version

        // Add colorbar - might need adjustment
        try
        {
            var colorbar = plt.Add.ColorBar(heatmap);
            colorbar.Label = "Magnitude (dB)";
        }
        catch (Exception)
        {
            // Skip if this method isn't available in your version
        }

        // Set labels
        plt.Title("Audio Spectrogram");
        plt.XLabel("Time (seconds)");
        plt.YLabel("Frequency (kHz)");

        // Set axis properties, using try/catch to handle version differences
        try
        {
            // Set custom ticks for frequency axis (convert bin index to kHz)
            // double maxFreqKhz = signal.SamplingRate / 2000.0; // Nyquist in kHz
            // double durationSec = (double)signal.Length / signal.SamplingRate;

            // // Add custom tick labels
            // for (int i = 0; i <= 10; i++) {
            //     double freqPct = i / 10.0;
            //     double freqKhz = freqPct * maxFreqKhz;
            //     double yPos = freqPct * (windowSize / 2 - 1);

            //     plt.Add.Text($"{freqKhz:F1}", -5, yPos);
            // }

            // // Add time labels along x-axis
            // for (int i = 0; i <= 10; i++) {
            //     double timePct = i / 10.0;
            //     double timeSec = timePct * durationSec;
            //     double xPos = timePct * (hopCount - 1);

            //     plt.Add.Text($"{timeSec:F1}", xPos, -5);
            // }
        }
        catch (Exception)
        {
            // Skip custom axis formatting if not supported
        }

        // Ensure the directory exists
        Directory.CreateDirectory(Path.GetDirectoryName(outputPath) ?? string.Empty);

        // Save the plot to a file
        plt.SavePng(outputPath, 1000, 600);

        return outputPath;
    }
}