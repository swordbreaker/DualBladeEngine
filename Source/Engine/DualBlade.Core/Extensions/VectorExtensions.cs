using System.Drawing;

namespace DualBlade.Core.Extensions;
public static class VectorExtensions
{
    public static Vector3 ToVector3(this Vector2 vector) =>
        new(vector.X, vector.Y, 0);

    public static PointF ToPointF(this Vector2 vector) =>
        new(vector.X, vector.Y);
}
