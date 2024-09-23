using DualBlade.Core.Shared.MonoGameInterfaces;

namespace DualBlade.Core.Desktop.Adapters;

public class Vector2Adapter : IVector2
{
    private readonly Vector2 _vector2;

    public Vector2Adapter(Vector2 vector2)
    {
        _vector2 = vector2;
    }

    public float X => _vector2.X;

    public float Y => _vector2.Y;

    public static implicit operator Vector2Adapter(Vector2 vector2) =>
        new(vector2);

    public static implicit operator Vector2(Vector2Adapter vector2) =>
        vector2._vector2;
}
