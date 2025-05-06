using System;
using System.IO;
using NWaves.Signals;
using OpenTK.Audio.OpenAL;

namespace BulletHell.Desktop.Services;

public class AudioService
{
    /// <summary>
    /// Loads an audio file and converts it to a DiscreteSignal
    /// </summary>
    /// <param name="filePath">Path to the audio file</param>
    /// <returns>DiscreteSignal representation of the audio file</returns>
    public DiscreteSignal LoadAudioFile(string filePath)
    {
        // Initialize OpenAL (we'll use this for playback in the future)
        ALDevice device = ALC.OpenDevice(null);
        ALContext context = ALC.CreateContext(device, new int[0]);
        ALC.MakeContextCurrent(context);

        try
        {
            string extension = Path.GetExtension(filePath).ToLower();
            DiscreteSignal signal;

            // Process different file types
            if (extension == ".mp3")
            {
                signal = LoadMp3File(filePath);
            }
            else if (extension == ".wav")
            {
                signal = LoadWavFile(filePath);
            }
            else
            {
                throw new NotSupportedException($"File format {extension} is not supported");
            }

            return signal;
        }
        finally
        {
            // Clean up OpenAL resources
            ALC.DestroyContext(context);
            ALC.CloseDevice(device);
        }
    }

    /// <summary>
    /// Loads an MP3 file using NLayer
    /// </summary>
    /// <param name="filePath">Path to the MP3 file</param>
    /// <returns>DiscreteSignal representation of the MP3 file</returns>
    static DiscreteSignal LoadMp3File(string filePath)
    {
        using (var mp3Stream = new FileStream(filePath, FileMode.Open))
        {
            var mp3Reader = new NLayer.MpegFile(mp3Stream);

            // Get audio format information
            int sampleRate = mp3Reader.SampleRate;
            int channels = mp3Reader.Channels;

            // Calculate total samples from duration (in milliseconds) and sample rate
            long totalSamples = (long)(mp3Reader.Duration.TotalSeconds * sampleRate * channels);
            float[] samples = new float[totalSamples];

            // Buffer for reading PCM samples
            float[] buffer = new float[4096];
            int sampleIndex = 0;
            int read;

            // Read all PCM samples
            while ((read = mp3Reader.ReadSamples(buffer, 0, buffer.Length)) > 0)
            {
                Array.Copy(buffer, 0, samples, sampleIndex, read);
                sampleIndex += read;
            }

            // If necessary, trim the samples array to the actual size read
            if (sampleIndex < totalSamples)
            {
                Array.Resize(ref samples, sampleIndex);
            }

            // Convert to mono if stereo
            if (channels > 1)
            {
                float[] monoSamples = new float[samples.Length / channels];
                for (int i = 0; i < monoSamples.Length; i++)
                {
                    float sum = 0;
                    for (int ch = 0; ch < channels; ch++)
                    {
                        sum += samples[i * channels + ch];
                    }
                    monoSamples[i] = sum / channels;
                }

                return new DiscreteSignal(sampleRate, monoSamples);
            }
            else
            {
                return new DiscreteSignal(sampleRate, samples);
            }
        }
    }

    /// <summary>
    /// Loads a WAV file manually parsing its format
    /// </summary>
    /// <param name="filePath">Path to the WAV file</param>
    /// <returns>DiscreteSignal representation of the WAV file</returns>
    static DiscreteSignal LoadWavFile(string filePath)
    {
        using (FileStream fileStream = File.OpenRead(filePath))
        using (BinaryReader reader = new BinaryReader(fileStream))
        {
            // Simple WAV file format parsing (this is a basic implementation)
            // Read RIFF header
            string riffHeader = new string(reader.ReadChars(4));
            if (riffHeader != "RIFF")
                throw new Exception("Not a valid WAV file - missing RIFF header");

            // Skip file size
            reader.ReadInt32();

            // Check format
            string waveHeader = new string(reader.ReadChars(4));
            if (waveHeader != "WAVE")
                throw new Exception("Not a valid WAV file - missing WAVE header");

            // Find the "fmt " chunk
            while (true)
            {
                string chunkId = new string(reader.ReadChars(4));
                int chunkSize = reader.ReadInt32();

                if (chunkId == "fmt ")
                {
                    // Read format data
                    int formatTag = reader.ReadInt16();
                    int channels = reader.ReadInt16();
                    int sampleRate = reader.ReadInt32();
                    reader.ReadInt32(); // Bytes per second
                    reader.ReadInt16(); // Block align
                    int bitsPerSample = reader.ReadInt16();

                    // Skip any extra data in the format chunk
                    if (chunkSize > 16)
                        reader.ReadBytes(chunkSize - 16);

                    // Find the "data" chunk
                    while (true)
                    {
                        string dataChunkId = new string(reader.ReadChars(4));
                        int dataChunkSize = reader.ReadInt32();

                        if (dataChunkId == "data")
                        {
                            // Read the audio data
                            byte[] audioBytes = reader.ReadBytes(dataChunkSize);

                            // Convert bytes to float samples based on bit depth
                            float[] samples = new float[audioBytes.Length / (bitsPerSample / 8)];

                            if (bitsPerSample == 16)
                            {
                                for (int i = 0; i < samples.Length; i++)
                                {
                                    short sample = (short)((audioBytes[i * 2 + 1] << 8) | audioBytes[i * 2]);
                                    samples[i] = sample / 32768.0f;
                                }
                            }
                            else if (bitsPerSample == 8)
                            {
                                for (int i = 0; i < samples.Length; i++)
                                {
                                    samples[i] = (audioBytes[i] - 128) / 128.0f;
                                }
                            }
                            else
                            {
                                throw new NotSupportedException($"Bit depth {bitsPerSample} is not supported");
                            }

                            // If stereo, convert to mono
                            if (channels > 1)
                            {
                                float[] monoSamples = new float[samples.Length / channels];
                                for (int i = 0; i < monoSamples.Length; i++)
                                {
                                    float sum = 0;
                                    for (int ch = 0; ch < channels; ch++)
                                    {
                                        sum += samples[i * channels + ch];
                                    }
                                    monoSamples[i] = sum / channels;
                                }

                                return new DiscreteSignal(sampleRate, monoSamples);
                            }
                            else
                            {
                                return new DiscreteSignal(sampleRate, samples);
                            }
                        }
                        else
                        {
                            // Skip this chunk
                            reader.ReadBytes(dataChunkSize);
                        }
                    }
                }
                else
                {
                    // Skip this chunk
                    reader.ReadBytes(chunkSize);
                }
            }
        }
    }
}
