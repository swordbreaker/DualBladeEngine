namespace DualBlade.Core.Systems;
public abstract class FixedSystem : ISystem
{
    public abstract void Update(GameTime gameTime);
    public void Draw(GameTime gameTime) { }
    public virtual void Initialize() { }
    public void Dispose() { }
}
