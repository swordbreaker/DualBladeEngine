using DualBlade.Core.Shared.MonoGameInterfaces;
namespace DualBlade.Core.Desktop.Adapters;

public class GameTimeAdapter : IGameTime
{
    private readonly GameTime _gameTime;

    public GameTimeAdapter(GameTime gameTime)
    {
        _gameTime = gameTime;
    }

    public TimeSpan ElapsedGameTime => _gameTime.ElapsedGameTime;

    public TimeSpan TotalGameTime => _gameTime.TotalGameTime;

    public static implicit operator GameTimeAdapter(GameTime gameTime) =>
        new(gameTime);

    public static implicit operator GameTime(GameTimeAdapter gameTime) =>
        gameTime._gameTime;
}
