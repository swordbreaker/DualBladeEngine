using DualBlade.Analyzer;
using DualBlade.Core.Entities;
using DualBlade.MyraUi.Components;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.Properties;
using System;
using System.Diagnostics;

namespace DualBlade.Editor.Player.Entities;

public class EditorUiEntity : NodeEntity
{
    public EditorUiEntity(SceneRoot sceneRoot, Action onChnaged)
    {
        var root = new Panel();

        var propertyGrid = new PropertyGrid()
        {
            Object = sceneRoot,

        };

        propertyGrid.PropertyChanged += (sender, e) => 
            Debug.WriteLine(e.Data);

        var window = new Window
        {
            Title = "Object Editor",
            Content = propertyGrid,
            MaxHeight = 800,
            MaxWidth = 800,
        };

        var desktop = new Desktop();
        desktop.Root = root;
        window.ShowModal(desktop);
        AddComponent(new MyraDesktopComponent() { Desktop = desktop, Entity = this });
    }

    private void PropertyGrid_PropertyChanged(object sender, Myra.Events.GenericEventArgs<string> e) => throw new NotImplementedException();
}
