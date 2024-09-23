using DualBlade.Core.Components;
using Myra.Graphics2D.UI;

namespace DualBlade.MyraUi.Components;

public class MyraDesktopComponent : NodeComponent
{
    public Desktop Desktop { get; set; } = new();
}
