namespace MonoGameEngine.Engine.Extensions;
public static class GameTimeExtensions
{
    public static float DeltaSeconds(this GameTime gameTime) =>
        (float)gameTime.ElapsedGameTime.TotalSeconds;
}
