using DualBlade.Core.Components;
using Myra.Graphics2D.UI;

namespace DualBlade.MyraUi.Components;

public partial struct MyraDesktopComponent : IComponent
{
    public Desktop Desktop = new();
}
