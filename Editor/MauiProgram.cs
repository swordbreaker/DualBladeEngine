using Microsoft.Extensions.Logging;
using MonoGameEngine;
using MonoGameEngine.Engine.Physics;
using MonoGameEngine.Engine.Rendering;
using MonoGameEngine.Engine.Services;
using MonoGameEngine.Engine.Systems;
using MonoGameEngine.Engine.Worlds;

namespace Editor;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services
            .AddSingleton<InputManager>()
            .AddSingleton<IGameEngineFactory, GameEngineFactory>()
            .AddSingleton<IWorldFactory, WorldFactory>()
            .AddSingleton<IPhysicsManager, PhysicsManager>()
            .AddSingleton<ISystemFactory, SystemFactory>()
            .AddSingleton<ISpriteFactory, SpriteFactory>()
            .AddSingleton<ICameraServiceFactory, CameraServiceFactory>()
            .AddSingleton<IWorldToPixelConverterFactory, WorldToPixelConverterFactory>()
            .AddSingleton<MainGame>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
