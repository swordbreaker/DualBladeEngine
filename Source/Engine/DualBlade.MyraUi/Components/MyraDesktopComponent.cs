using DualBlade.Core.Components;
using DualBlade.Core.Entities;
using Myra.Graphics2D.UI;

namespace DualBlade.MyraUi.Components;

public class MyraDesktopComponent : IComponent
{
    public Desktop Desktop { get; set; } = new();
    public IEntity Entity { get; init; }
}
