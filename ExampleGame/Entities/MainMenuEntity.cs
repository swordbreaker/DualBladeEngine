using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using DualBlade.GumUi.Components;
using ExampleGame.Scenes;
using Gum.Converters;
using MonoGameGum.Forms.Controls;
using MonoGameGum.GueDeriving;
using RenderingLibrary.Graphics;

namespace ExampleGame.Entities;

internal class MainMenuEntity : Entity
{
    public MainMenuEntity(IGameContext gameContext, IJobQueue jobQueue)
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

        startButton.Click += (s, e) =>
        {
            jobQueue.Enqueue(() => gameContext.SceneManager.AddSceneExclusively<MainScene>());
        };

        quitButton.Click += (s, e) => gameContext.Game.Exit();

        root.Children.Add(startButton.Visual);
        root.Children.Add(quitButton.Visual);

        this.AddComponent(new UiCanvasComponent() { Entity = this, Container = root });
    }
}