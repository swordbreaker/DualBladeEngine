using DualBlade.Core.Rendering;
using DualBlade.Core.Services;
using Microsoft.Xna.Framework.Graphics;

namespace DualBlade.Core.Factories;

public sealed class SpriteFactory(IWorldToPixelConverter _worldToPixelConverter) : ISpriteFactory
{
    public ISprite CreateSprite(Texture2D texture2D) =>
        new Sprite(_worldToPixelConverter) { Texture2D = texture2D };
}
