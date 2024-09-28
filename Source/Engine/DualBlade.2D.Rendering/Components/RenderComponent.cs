using DualBlade.Core.Components;
using DualBlade.Core.Rendering;

namespace DualBlade._2D.Rendering.Components;

public partial struct RenderComponent : IComponent
{
    public Color Color = Color.White;
    public ISprite Sprite;
    public Vector2 Origin = Vector2.Zero;

    public void SetSprite(ISprite sprite, bool updateOrigin = true)
    {
        Sprite = sprite;
        if (updateOrigin)
        {
            Origin = new Vector2(sprite.Width / 2f, sprite.Height / 2f);
        }
    }
}
