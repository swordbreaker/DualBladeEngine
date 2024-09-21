using MonoGameEngine.Engine.Entities;
using MonoGameEngine.Ui.Components;
using MonoGameGum.GueDeriving;
namespace MonoGameEngine.Ui.Entities;

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
