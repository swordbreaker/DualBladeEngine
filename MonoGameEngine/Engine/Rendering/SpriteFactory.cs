using Microsoft.Xna.Framework.Graphics;
using MonoGameEngine.Engine.Services;

namespace MonoGameEngine.Engine.Rendering;
public class SpriteFactory(IWorldToPixelConverter _worldToPixelConverter) : ISpriteFactory
{
    public ISprite CreateSprite(Texture2D texture2D) =>
        new Sprite(_worldToPixelConverter) { Texture2D = texture2D };
}
