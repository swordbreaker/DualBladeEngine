using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.MyraUi.Components;
using ExampleGame.Scenes;
using Myra.Graphics2D;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.Styles;
using FontStashSharp.RichText;

namespace ExampleGame.Entities;

public partial struct MainMenuEntity : IEntity
{
    public MainMenuEntity(IGameContext gameContext, IJobQueue jobQueue)
    {
        Stylesheet.Current.ButtonStyle.Padding = new Thickness(10);

        // UI Presets
        static Label CreateLabel(string text) => new() { Text = text, TextAlign = TextHorizontalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center };
        static Button CreateButton(string text) => new() { Content = CreateLabel(text), Width = 200 };

        // UI Elements
        var startButton = CreateButton("Start");
        var quitButton = CreateButton("Quit");

        var stackPanel = new VerticalStackPanel
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Spacing = 10,
        };

        // Add Child Elements
        stackPanel.Widgets.Add(startButton);
        stackPanel.Widgets.Add(quitButton);

        // Hook up events
        startButton.Click += (sender, args) =>
            jobQueue.Enqueue(() => gameContext.SceneManager.AddSceneExclusively(new MainScene(gameContext)));

        quitButton.Click += (sender, args) =>
            gameContext.Game.Exit();

        var desktop = new Desktop
        {
            Root = stackPanel,
        };

        this.AddComponent(new MyraDesktopComponent() { Desktop = desktop });
    }
}