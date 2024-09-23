namespace DualBlade.Core.Systems;

public interface ISystem : IDisposable
{
    void Update(GameTime gameTime);

    void Draw(GameTime gameTime);
    void Initialize();
}
