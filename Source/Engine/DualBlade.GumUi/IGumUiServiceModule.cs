using DualBlade.Core.Services;
using DualBlade.GumUi.Systems;
using Jab;

namespace DualBlade.GumUi;

[ServiceProviderModule]
[Singleton<IStartupService, UiStartupService>]
[Transient<UiCanvasSystem>]
public interface IGumUiServiceModule
{
}
