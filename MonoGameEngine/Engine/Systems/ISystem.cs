using MonoGameEngine.Engine.Worlds;
using System;

namespace MonoGameEngine.Engine.Systems;

public interface ISystem : IDisposable
{
    void Update(GameTime gameTime, IGameEngine gameEngine);

    void Draw(GameTime gameTime, IGameEngine gameEngine);
    void Initialize(IGameEngine gameEngine);
}
