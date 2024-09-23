using DualBlade.Core.Components;
using MonoGameGum.GueDeriving;

namespace DualBlade.GumUi.Components;

public class UiCanvasComponent : NodeComponent
{
    public required ContainerRuntime Container { get; init; }
}
