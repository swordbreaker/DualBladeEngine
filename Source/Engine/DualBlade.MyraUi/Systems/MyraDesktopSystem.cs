using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using DualBlade.MyraUi.Components;
using Microsoft.Xna.Framework;

namespace DualBlade.MyraUi.Systems;
public class MyraDesktopSystem(IGameContext gameContext) : ComponentSystem<MyraDesktopComponent>(gameContext)
{
    protected override void Draw(MyraDesktopComponent component, IEntity entity, GameTime gameTime)
    {
        component.Desktop.Render();
    }
}
