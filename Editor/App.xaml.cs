namespace Editor;

public partial class App : Application
{
    public App(MainGame mainGame)
    {
        InitializeComponent();

        MainPage = new AppShell(mainGame);
    }
}
