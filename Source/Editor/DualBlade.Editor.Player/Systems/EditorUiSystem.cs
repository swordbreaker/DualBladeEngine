using DualBlade.Core.Extensions;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using DualBlade.Editor.Player.Entities;
using DualBlade.Editor.Player.Services;
using DualBlade.MyraUi.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DualBlade.Editor.Player.Systems;
public class EditorUiSystem(IGameContext context, SystemProvider systemProvider, IJobQueue jobQueue) : EntitySystem<EditorUiEntity>(context)
{
    private readonly IInputManager input = context.GameEngine.InputManager;

    protected override void Update(ref EditorUiEntity entity, GameTime gameTime)
    {
        if (input.IsKeyJustPressed(Keys.Home) && !entity.ControlWindow.Visible)
        {
            if (entity.TryGetComponent<MyraDesktopComponent>(out var myraDesktopComponent))
            {
                entity.ControlWindow.Show(myraDesktopComponent.Desktop);
            }
        }

        entity.SystemStackPanel.Widgets.Clear();

        foreach (var system in systemProvider.Systems)
        {
            var button = new ToggleButton
            {
                Content = new Label { Text = system.GetType().Name },
                Width = 200,
                Height = 50,
                IsToggled = GameContext.World.Systems.Contains(system),
            };

            button.IsToggledChanged += (sender, e) =>
            {
                if (button.IsToggled)
                {
                    jobQueue.Enqueue(() => GameContext.World.AddSystem(system));
                }
                else
                {
                    jobQueue.Enqueue(() => GameContext.World.Destroy(system));
                }
            };

            entity.SystemStackPanel.Widgets.Add(button);
        }
    }

    private IEnumerable<Type> GetAllSystems()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .ToHashSet()
            .Where(x => x.IsSubclassOf(typeof(ISystem)))
            .ToList();
    }
}
