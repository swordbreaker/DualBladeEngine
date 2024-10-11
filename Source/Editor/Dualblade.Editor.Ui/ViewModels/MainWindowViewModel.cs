using CommunityToolkit.Mvvm.Input;
using Editor;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Windows.Input;

namespace Dualblade.Editor.Ui.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
#pragma warning disable CA1822 // Mark members as static
    public string Greeting => "Welcome to Avalonia!";
#pragma warning restore CA1822 // Mark members as static

    public ICommand StartCommand { get; } = new RelayCommand(() =>
    {
        var assembly = Assembly.LoadFrom("FluidBattle.dll");

        var provider = new ServiceProvider();
        var game = provider.GetRequiredService<EditorGame>();
        game.Run();
    });
}
