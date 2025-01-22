using System.Drawing;

namespace DualBlade.Core.Extensions;

public static class VectorExtensions
{
    /// <summary>
    /// Converts a Vector2 to a Vector3.
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public static Vector3 ToVector3(this Vector2 vector) =>
        new(vector.X, vector.Y, 0);

    /// <summary>
    /// Converts a Vector2 to a PointF.
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public static PointF ToPointF(this Vector2 vector) =>
        new(vector.X, vector.Y);
}