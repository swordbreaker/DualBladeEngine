namespace DualBlade._2D.BladePhysics.Services;
public static class Physics
{
    public static bool LineCircleCollision(Vector2 lineStart, Vector2 lineEnd, Vector2 circleCenter, float radius, out Vector2 collisionPoint)
    {
        collisionPoint = Vector2.Zero;
        var d = lineEnd - lineStart;
        var f = lineStart - circleCenter;

        var a = Vector2.Dot(d, d);
        var b = 2 * Vector2.Dot(f, d);
        var c = Vector2.Dot(f, f) - radius * radius;

        var discriminant = b * b - 4 * a * c;
        if (discriminant < 0)
        {
            return false;
        }

        discriminant = MathF.Sqrt(discriminant);

        var t1 = (-b - discriminant) / (2 * a);
        var t2 = (-b + discriminant) / (2 * a);

        if (t1 >= 0 && t1 <= 1)
        {
            collisionPoint = lineStart + t1 * d;
            return true;
        }

        if (t2 >= 0 && t2 <= 1)
        {
            collisionPoint = lineStart + t2 * d;
            return true;
        }

        return false;
    }
}
