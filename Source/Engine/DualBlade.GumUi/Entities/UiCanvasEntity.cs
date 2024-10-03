using DualBlade.Core.Entities;
using DualBlade.GumUi.Components;
using MonoGameGum.GueDeriving;
namespace DualBlade.GumUi.Entities;

public partial struct UiCanvasEntity : IEntity
{
    public UiCanvasEntity(ContainerRuntime containerRuntime)
    {
        var canvasComponent = new UiCanvasComponent()
        {
            Container = containerRuntime
        };

        this.AddComponent(canvasComponent);
    }
}
