using MonoGameEngine.Engine.Components;
using MonoGameGum.GueDeriving;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.S

namespace MonoGameEngine.Ui.Components;

public class UiCanvasComponent : NodeComponent
{
    public required ContainerRuntime Container { get; init; }
}
