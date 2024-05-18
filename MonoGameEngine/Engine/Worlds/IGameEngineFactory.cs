using Microsoft.Xna.Framework;

namespace MonoGameEngine.Engine.Worlds;

public interface IGameEngineFactory
{
    IGameEngine Create(Game game);
}