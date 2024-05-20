using Microsoft.Xna.Framework.Graphics;

namespace MonoGameEngine.Engine.Rendering;

public interface ISpriteFactory
{
    ISprite CreateSprite(Texture2D texture2D);
}