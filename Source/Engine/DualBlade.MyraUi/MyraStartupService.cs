using DualBlade.Core.Services;
using Myra;

namespace DualBlade.MyraUi;
public class MyraStartupService : IStartupService
{
    public void OnStart(IGameContext gameContext)
    {
        MyraEnvironment.Game = gameContext.Game;
    }
}
