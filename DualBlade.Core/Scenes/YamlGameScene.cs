using DualBlade.Core.Services;
using DualBlade.Core.Systems;

namespace DualBlade.Core.Scenes;

public abstract class YamlGameScene(IGameContext context) : GameScene(context)
{
    public override IEnumerable<ISystem> SetupSystems() => [];
}
