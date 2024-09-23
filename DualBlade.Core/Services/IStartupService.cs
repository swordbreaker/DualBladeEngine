namespace DualBlade.Core.Services;

public interface IStartupService
{
    void OnStart(IGameContext gameContext);
}
