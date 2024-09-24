using DualBlade.Core.Services;
using Microsoft.Xna.Framework.Content;

namespace DualBlade.Core.Factories;

public interface IGameEngineFactory
{
    IGameEngine Create(Game game, ContentManager? contentManager = null);
}