using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Microsoft.Xna.Framework;
using ExampleGame;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using DualBlade.Core;
using DualBlade.GumUi;
using DualBlade._2D.Rendering;

namespace ExampleAndroid
{
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

            _game = CreateGame();
            //_game = new Game1();
            _view = _game.Services.GetService(typeof(View)) as View;

            SetContentView(_view);
            _game.Run();
        }

        private static MainGame CreateGame()
        {
            var host = new HostBuilder()
            .AddGameEngine()
            .AddGumUi()
            .AddPhysics2D()
            .Add2DRendering()
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<MainGame>();
            })
            .Build();

            return host.Services.GetRequiredService<MainGame>();
        }
    }
}
