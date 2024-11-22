namespace DualBlade.Core.Models;

public struct Vector2i
{
    public int X;
    public int Y;

    public Vector2i(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static Vector2i Zero => new(0, 0);
    public static Vector2i One => new(1, 1);

    public static Vector2i operator +(Vector2i a, Vector2i b) =>
        new(a.X + b.X, a.Y + b.Y);

    public static Vector2i operator -(Vector2i a, Vector2i b) =>
        new(a.X - b.X, a.Y - b.Y);

    public static Vector2i operator *(Vector2i a, int b) =>
        new(a.X * b, a.Y * b);

    public static Vector2i operator /(Vector2i a, int b) =>
        new(a.X / b, a.Y / b);

    public static bool operator ==(Vector2i a, Vector2i b) =>
        a.X == b.X && a.Y == b.Y;

    public static bool operator !=(Vector2i a, Vector2i b) =>
        a.X != b.X || a.Y != b.Y;

    public static implicit operator Vector2(Vector2i vector) =>
        new(vector.X, vector.Y);

    public static implicit operator Vector2i(Vector2 vector) =>
        new((int)vector.X, (int)vector.Y);

    public override bool Equals(object obj) =>
        obj is Vector2i vector && vector == this;

    public override int GetHashCode() =>
        HashCode.Combine(X, Y);

    public override string ToString() =>
        $"({X}, {Y})";
}
