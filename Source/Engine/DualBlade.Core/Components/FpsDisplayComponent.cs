using Microsoft.Xna.Framework.Graphics;

namespace DualBlade.Core.Components;

public class FpsDisplayComponent : ComponentBase
{
    public int FrameCounter;
    public int FrameRate;
    public TimeSpan ElapsedFrameTime = TimeSpan.Zero;

    public int UpdateCounter;
    public int UpdateRate;
    public TimeSpan ElapsedUpdateTime = TimeSpan.Zero;

    public SpriteFont? Font;
    public Vector2 Position = new(10, 10);
    public Color FontColor = Color.Black;
}
