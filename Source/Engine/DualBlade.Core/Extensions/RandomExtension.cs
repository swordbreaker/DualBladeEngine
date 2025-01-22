namespace DualBlade.Core.Extensions;

public static class RandomExtension
{
    /// <summary>
    /// Get a random float between 0 and 1.
    /// </summary>
    /// <param name="random">The random instance.</param>
    /// <param name="min">Inclusive min value.</param>
    /// <param name="max">Exclusive max value.</param>
    /// <returns>This will return a value greater equals to min and less than max.</returns>
    public static float NextFloat(this Random random, float min, float max)
    {
        return (float)random.NextSingle() * (max - min) + min;
    }
}