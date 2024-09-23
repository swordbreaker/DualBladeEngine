using DualBlade.Core.Entities;
using DualBlade.Core.Scenes;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using DualBlade.GumUi.Systems;
using ExampleGame.Entities;

namespace ExampleGame.Scenes;
public class MainMenuScene(IGameContext _gameContext) : GameScene(_gameContext)
{
    public override IEnumerable<ISystem> SetupSystems()
    {
        yield return this.CreateSystem<UiCanvasSystem>();
    }

    protected override IEnumerable<IEntity> SetupEntities()
    {
        yield return CreateEntity<MainMenuEntity>();
    }

    private void StartButton_Click(object? sender, EventArgs e) => throw new NotImplementedException();
}
