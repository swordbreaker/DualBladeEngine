using DualBlade.Core.Shared.MonoGameInterfaces;
using Microsoft.Xna.Framework.Graphics;
namespace DualBlade.Core.Desktop.Adapters;
public class Texture2DAdapter : ITexture2D
{
    private readonly Texture2D _texture2D;

    public Texture2DAdapter(Texture2D texture2D)
    {
        _texture2D = texture2D;
    }

    public static implicit operator Texture2DAdapter(Texture2D texture2D) =>
        new(texture2D);

    public static implicit operator Texture2D(Texture2DAdapter texture2D) =>
        texture2D._texture2D;
}
