namespace Editor;

public partial class AppShell : Shell
{
    public AppShell(MainGame mainGame)
    {
        InitializeComponent();
        this.ShellContent.Content = new MainPage(mainGame);
    }
}
