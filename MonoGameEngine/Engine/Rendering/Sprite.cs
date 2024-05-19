using Microsoft.Xna.Framework.Graphics;
using MonoGameEngine.Engine.Services;

namespace MonoGameEngine.Engine.Rendering;
public class Sprite(IWorldToPixelConverter _worldToPixelConverter) : ISprite
{
    public Texture2D? Texture2D { get; init; }

    public Vector2 Size => _worldToPixelConverter.WorldSizeToPixel(new(Texture2D?.Width ?? 0, Texture2D?.Height ?? 0));
}
