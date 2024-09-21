using Gum.Converters;
using MonoGameEngine.Engine.Entities;
using MonoGameEngine.Engine.Scenes;
using MonoGameEngine.Engine.Services;
using MonoGameEngine.Engine.Systems;
using MonoGameEngine.Engine.Worlds;
using MonoGameEngine.Ui.Components;
using MonoGameEngine.Ui.Systems;
using MonoGameGum.Forms.Controls;
using MonoGameGum.GueDeriving;
using RenderingLibrary.Graphics;

namespace ExampleGame.Scenes;
public class MainMenuScene(IWorld world, IGameEngine gameEngine, ISceneManager sceneManager, IJobQueue jobQueue) : GameScene(world)
{
    public override IEnumerable<ISystem> SetupSystems()
    {
        yield return this.CreateSystem<UiCanvasSystem>();
    }

    protected override IEnumerable<IEntity> SetupEntities()
    {
        var root = new ContainerRuntime
        {
            ChildrenLayout = Gum.Managers.ChildrenLayout.TopToBottomStack,
            XOrigin = HorizontalAlignment.Center,
            XUnits = GeneralUnitType.PixelsFromMiddle,
            YOrigin = VerticalAlignment.Center,
            YUnits = GeneralUnitType.PixelsFromMiddle,
            StackSpacing = 10,
        };

        var startButton = new Button()
        {
            Visual =
            {
                XOrigin = HorizontalAlignment.Center,
                XUnits = GeneralUnitType.PixelsFromMiddle,
                WidthUnits = Gum.DataTypes.DimensionUnitType.AbsoluteMultipliedByFontScale
            },
            Width = 200,
            Height = 50,
            Text = "Start"
        };

        startButton.Click += (s, e) =>
        {
            jobQueue.Enqueue(() => sceneManager.AddSceneExclusively(new MainScene(World, gameEngine)));
        };

        var quitButton = new Button()
        {
            Visual =
            {
                XOrigin = HorizontalAlignment.Center,
                XUnits = GeneralUnitType.PixelsFromMiddle,
                WidthUnits = Gum.DataTypes.DimensionUnitType.AbsoluteMultipliedByFontScale
            },
            Width = 200,
            Height = 50,
            Text = "Quit"
        };

        root.Children.Add(startButton.Visual);
        root.Children.Add(quitButton.Visual);

        var mainMenuEntity = new Entity() { World = World };
        mainMenuEntity.AddComponent(
            new UiCanvasComponent() { Container = root, Entity = mainMenuEntity });

        yield return mainMenuEntity;
    }

    private void StartButton_Click(object? sender, EventArgs e) => throw new NotImplementedException();
}
