using MonoGameEngine.Engine.Worlds;

namespace MonoGameEngine.Engine.Systems;

public interface ISystem
{
    void Update(GameTime gameTime, IGameEngine gameEngine);

    void Draw(GameTime gameTime, IGameEngine gameEngine);
    void Initialize(IGameEngine gameEngine);
}
