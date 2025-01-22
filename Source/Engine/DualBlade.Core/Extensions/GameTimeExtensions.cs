namespace DualBlade.Core.Extensions;

public static class GameTimeExtensions
{
    /// <summary>
    /// Get the delta time in seconds.
    /// </summary>
    /// <param name="gameTime"></param>
    /// <returns></returns>
    public static float DeltaSeconds(this GameTime gameTime) =>
        (float)gameTime.ElapsedGameTime.TotalSeconds;
}