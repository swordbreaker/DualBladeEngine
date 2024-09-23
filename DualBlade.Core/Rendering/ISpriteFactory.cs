using Microsoft.Xna.Framework.Graphics;

namespace DualBlade.Core.Rendering;

public interface ISpriteFactory
{
    ISprite CreateSprite(Texture2D texture2D);
}