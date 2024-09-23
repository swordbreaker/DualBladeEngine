using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using DualBlade.MyraUi.Components;
using Microsoft.Xna.Framework;

namespace DualBlade.MyraUi.Systems;
public class MyraDesktopSystem(IGameContext gameContext) : ComponentSystem<MyraDesktopComponent>(gameContext)
{
    protected override void Draw(MyraDesktopComponent component, GameTime gameTime)
    {
        base.Draw(component, gameTime);
        component.Desktop.Render();
    }
}
