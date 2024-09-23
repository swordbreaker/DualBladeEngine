using DualBlade.Core.Services;
using DualBlade.MyraUi;
using DualBlade.MyraUi.Systems;
using Jab;

namespace DualBlade.MyraUi;

[ServiceProviderModule]
[Singleton<IStartupService, MyraStartupService>]
[Transient<MyraDesktopSystem>]
public interface IMyraUiServiceModule
{
}
