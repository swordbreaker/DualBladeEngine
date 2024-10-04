using DualBlade.Core.Entities;
using DualBlade.MyraUi.Components;
using Myra.Graphics2D.UI.Styles;
using Myra.Graphics2D.UI;
using DualBlade.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Myra.Graphics2D;
using FontStashSharp.RichText;
using FluidBattle.Scenes;

namespace FluidBattle.Entities;
public partial struct MainMenuEntity : IEntity
{
    public MainMenuEntity(IGameContext gameContext)
    {
        var jobQueue = gameContext.ServiceProvider.GetRequiredService<IJobQueue>();

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
