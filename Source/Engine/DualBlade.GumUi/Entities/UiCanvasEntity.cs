using DualBlade.Core.Entities;
using DualBlade.GumUi.Components;
using MonoGameGum.GueDeriving;
namespace DualBlade.GumUi.Entities;

public class UiCanvasEntity : Entity
{
    public UiCanvasEntity(ContainerRuntime containerRuntime)
    {
        var canvasComponent = new UiCanvasComponent()
        {
            Entity = this,
            Container = containerRuntime
        };

        this.AddComponent(canvasComponent);
    }
}
