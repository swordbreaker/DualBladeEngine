using MonoGameEngine.Engine.Services;

namespace MonoGameEngine.Engine.Rendering;
public class SpriteFactory(IWorldToPixelConverter _worldToPixelConverter) : ISpriteFactory
{
    public Sprite CreateSprite()
    {
        return new Sprite(_worldToPixelConverter);
    }
}
