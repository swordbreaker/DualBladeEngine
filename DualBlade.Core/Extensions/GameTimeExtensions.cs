namespace DualBlade.Core.Extensions;
public static class GameTimeExtensions
{
    public static float DeltaSeconds(this GameTime gameTime) =>
        (float)gameTime.ElapsedGameTime.TotalSeconds;
}
