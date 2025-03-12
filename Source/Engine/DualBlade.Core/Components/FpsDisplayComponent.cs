using Microsoft.Xna.Framework.Graphics;

namespace DualBlade.Core.Components;

/// <summary>
/// Component that displays frame rate (FPS) and update rate information.
/// Tracks and shows performance metrics to help monitor game performance.
/// </summary>
public partial struct FpsDisplayComponent : IComponent
{
    /// <summary>
    /// Counter for tracking the number of frames rendered in the current measurement interval.
    /// </summary>
    public int FrameCounter;

    /// <summary>
    /// The calculated frames per second (FPS) value that gets displayed.
    /// </summary>
    public int FrameRate;

    /// <summary>
    /// Accumulated time since the last FPS calculation.
    /// Used to determine when to calculate a new FPS value.
    /// </summary>
    public TimeSpan ElapsedFrameTime = TimeSpan.Zero;

    /// <summary>
    /// Counter for tracking the number of update cycles in the current measurement interval.
    /// </summary>
    public int UpdateCounter;

    /// <summary>
    /// The calculated updates per second rate that gets displayed.
    /// </summary>
    public int UpdateRate;

    /// <summary>
    /// Accumulated time since the last update rate calculation.
    /// Used to determine when to calculate a new update rate value.
    /// </summary>
    public TimeSpan ElapsedUpdateTime = TimeSpan.Zero;

    /// <summary>
    /// Font used to render the FPS and update rate text.
    /// </summary>
    public SpriteFont? Font;

    /// <summary>
    /// Screen position where the FPS and update rate information will be displayed.
    /// </summary>
    public Vector2 Position = new(10, 10);

    /// <summary>
    /// Color of the text used to display the FPS and update rate information.
    /// </summary>
    public Color FontColor = Color.Black;
}