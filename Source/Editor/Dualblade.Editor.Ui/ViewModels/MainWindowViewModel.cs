using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.Input;
using Editor;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Windows.Input;

namespace Dualblade.Editor.Ui.ViewModels;

public partial class MainWindowViewModel(TopLevel topLevel) : ViewModelBase
{
#pragma warning disable CA1822 // Mark members as static
    public string Greeting => "Welcome to Avalonia!";
#pragma warning restore CA1822 // Mark members as static

    public ICommand StartCommand { get; } = new RelayCommand(async () =>
    {
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(
            new(){
                Title = "Open Assembly",
                FileTypeFilter = [new("dll")]
            }
        );

        Assembly.LoadFrom(files[0].Path.AbsolutePath);

        var provider = new ServiceProvider();
        var game = provider.GetRequiredService<EditorGame>();
        game.Run();
    });
}
