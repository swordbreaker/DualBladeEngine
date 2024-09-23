namespace DualBlade.Core.Extensions;
public static class VectorExtensions
{
    public static Vector3 ToVector3(this Vector2 vector) =>
        new Vector3(vector.X, vector.Y, 0);
}
