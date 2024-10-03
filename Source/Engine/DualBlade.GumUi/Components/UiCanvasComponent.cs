using DualBlade.Core.Components;
using MonoGameGum.GueDeriving;

namespace DualBlade.GumUi.Components;


public partial struct UiCanvasComponent : IComponent
{
    public required ContainerRuntime Container { get; init; }
}
