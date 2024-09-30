using DualBlade.Core.Services;
using DualBlade.Core.Worlds;

namespace DualBlade.Core.Systems;

public class BaseSystem(IGameContext gameContext) : ISystemWithContext
{
    public IGameContext GameContext { get; } = gameContext;
    public IWorld World { get; } = gameContext.World;

    protected IEcsManager Ecs = gameContext.EcsManager;

    public virtual void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public virtual void Draw(GameTime gameTime) { }
    public virtual void Initialize() { }
    public virtual void Update(GameTime gameTime) { }
}
