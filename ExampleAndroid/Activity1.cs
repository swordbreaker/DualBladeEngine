using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using ExampleGame;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Xna.Framework;

namespace ExampleAndroid;

[Activity(
    Label = "@string/app_name",
    MainLauncher = true,
    Icon = "@drawable/icon",
    AlwaysRetainTaskState = true,
    LaunchMode = LaunchMode.SingleInstance,
    ScreenOrientation = ScreenOrientation.FullUser,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize
)]
public class Activity1 : AndroidGameActivity
{
    private MainGame _game;
    private View _view;

    protected override void OnCreate(Bundle bundle)
    {
        base.OnCreate(bundle);

        try
        {
            // var host = new HostBuilder()
            //     .AddGameEngine()
            //     .ConfigureServices((context, services) =>
            //     {
            //         services.AddSingleton<MainGame>();
            //     })
            //     .Build();

            // var game = host.Services.GetRequiredService<MainGame>();
            _game = new MainGame();
            _view = _game.Services.GetService(typeof(View)) as View;

            SetContentView(_view);
            _game.Run();
        }
        catch (System.Exception ex)
        {
            View view = new(this);
            view.SetBackgroundColor(Android.Graphics.Color.Red);
            System.Diagnostics.Debug.WriteLine(ex.Message);

            SetContentView(view);
        }
    }
}
