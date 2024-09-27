namespace DualBlade.Core.Extensions;
public static class RandomExtension
{
    public static float NextFloat(this Random random, float min, float max)
    {
        return (float)random.NextSingle() * (max - min) + min;
    }
}
