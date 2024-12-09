using Microsoft.Xna.Framework.Graphics;

namespace DualBlade.Core.Rendering;

public interface ISprite
{
    Texture2D Texture2D { get; init; }
    Vector2 Size { get; }
    public float Width => Size.X;
    public float Height => Size.Y;
}