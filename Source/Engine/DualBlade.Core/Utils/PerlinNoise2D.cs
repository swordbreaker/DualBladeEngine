namespace DualBlade.Core.Utils;
public class PerlinNoise2D
{
    private int[] permutation;
    private const int permutationSize = 256;

    public PerlinNoise2D(int seed)
    {
        Random random = new(seed);
        permutation = Enumerable.Range(0, permutationSize).OrderBy(x => random.Next()).ToArray();
        permutation = [.. permutation, .. permutation]; // Duplicate the permutation to avoid overflow
    }

    public float Noise(float x, float y)
    {
        int X = (int)Math.Floor(x) & 255;
        int Y = (int)Math.Floor(y) & 255;

        x -= (float)Math.Floor(x);
        y -= (float)Math.Floor(y);

        float u = Fade(x);
        float v = Fade(y);

        int A = permutation[X] + Y;
        int B = permutation[X + 1] + Y;

        return Lerp(v,
            Lerp(u, Grad(permutation[A], x, y),
                    Grad(permutation[B], x - 1, y)),
            Lerp(u, Grad(permutation[A + 1], x, y - 1),
                    Grad(permutation[B + 1], x - 1, y - 1)));
    }

    private static float Fade(float t) =>
        t * t * t * (t * (t * 6 - 15) + 10);

    private static float Lerp(float t, float a, float b) =>
        a + t * (b - a);

    private static float Grad(int hash, float x, float y)
    {
        int h = hash & 7;
        float u = h < 4 ? x : y;
        float v = h < 4 ? y : x;
        return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
    }
}
